using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LeaveManagementSystem.Application.Services.LeaveTypes;

public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper, ILogger<LeaveTypesService> _logger) : ILeaveTypesService
{
    //private readonly ApplicationDbContext _context = context;
    //private readonly IMapper _mapper = mapper;

    //public LeaveTypesService(ApplicationDbContext context, IMapper mapper)
    //{
    //    this.context = context;
    //    this.mapper = mapper;
    //}
    public async Task<List<LeaveTypeReadOnlyVM>> GetAllAsync()
    {
        var data = await _context.LeaveTypes.ToListAsync();
        //convert the data model into a view model
        /*
        var viewData = data.Select(q => new IndexVM
        {
            Id = q.Id,
            Name = q.Name,
            Days = q.NumberOfDays,
        });
        */
        var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
        //alternatively... var viewData = _mapper.Map<List<LeaveType>, List<IndexVM>>(data);

        //return the view model to the view
        //return View(data);
        return viewData;
    }

    public async Task<T?> GetAsync<T>(int id) where T : class
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (data == null)
        {
            return null;
        }
        var viewdata = _mapper.Map<T>(data);
        return viewdata;
    }

    public async Task RemoveAsync(int id)
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (data != null)
        {
            _context.Remove(data); //.LeaveTypes.Remove(data);
            await _context.SaveChangesAsync();
        }
    }

    public async Task EditAsync(LeaveTypeEditVM model)
    {
        try //174...although removed
        {
            var leaveType = _mapper.Map<LeaveType>(model);
            _context.Update(leaveType);
            await _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            //do something special with error then throw back to LeaveTypesController
            throw;
        }
    }

    public async Task CreateAsync(LeaveTypeCreateVM model)
    {
        _logger.LogInformation("Creating Leave Type: {leaveTypeName} - {days}", model.Name, model.Days); //176
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Add(leaveType);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsAsync(string name)
    {
        return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(name.ToLower()));
        //return _context.LeaveTypes.Any(q => q.Name.ToLower().Equals(name.ToLower()));
        //return _context.LeaveTypes.Any(q => q.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)); //currently problematic
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsForEditAsync(LeaveTypeEditVM leaveTypeEdit)
    {
        return await _context.LeaveTypes.AnyAsync(q => q.Name.ToLower().Equals(leaveTypeEdit.Name.ToLower())
            && q.Id != leaveTypeEdit.Id);
    }

    public async Task<bool> LeaveTypeExistsAsync(int id)
    {
        return await _context.LeaveTypes.AnyAsync(e => e.Id == id);
    }

    public async Task<bool> DaysExceedMaximumAsync(int leaveTypeID, int days)
    {
        var leaveType = await _context.LeaveTypes.FindAsync(leaveTypeID);
        return leaveType.NumberOfDays < days;
    }
}
