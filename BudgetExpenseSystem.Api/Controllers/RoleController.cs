using BudgetExpenseSystem.Domain.Domains;
using BudgetExpenseSystem.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace BudgetExpenseSystem.Api.Controllers;
[Route("api/[controller]s")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly RoleDomain _roleDomain;

    public RoleController(RoleDomain roleDomain)
    {
        _roleDomain = roleDomain;
    }

    [HttpGet]
    public async Task<ActionResult<List<Role>>> GetAllRoles()
    {
        return await _roleDomain.RetrieveAllRoles();
    }
    
}