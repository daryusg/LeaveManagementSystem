using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace LeaveManagementSystem.Web.Services.LeaveTypes;

public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper) : ILeaveTypesService
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
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Update(leaveType);
        await _context.SaveChangesAsync();
    }

    public async Task CreateAsync(LeaveTypeCreateVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Add(leaveType);
        await _context.SaveChangesAsync();
    }


    public bool LeaveTypeExists(int id)
    {
        return _context.LeaveTypes.Any(e => e.Id == id);
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
}
