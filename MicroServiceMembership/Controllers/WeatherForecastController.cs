using MicroserviceMembership.Application.Interfaces;
using MicroserviceMembership.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceMembership.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _membershipService.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(new { error = result.Error });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Membership membership)
        {
            var result = await _membershipService.CreateAsync(membership);
            if (result.IsFailure) return BadRequest(new { error = result.Error });

            var newMembershipResult = await _membershipService.GetByIdAsync(result.Value);
            return CreatedAtAction(nameof(GetById), new { id = result.Value }, newMembershipResult.Value);
        }
    }
}