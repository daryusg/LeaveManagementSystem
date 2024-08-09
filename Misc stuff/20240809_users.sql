select u.email, concat(u.FirstName, ' ', u.LastName) as Name , r.Name as role
from AspNetRoles r inner join AspNetUserRoles ur on r.id = ur.RoleId
	inner join AspNetUsers u on u.Id = ur.UserId
order by email