using Microsoft.AspNetCore.Mvc;
using MicroserviceMembership.Domain.Entities; // Asegúrate de que este namespace sea correcto
using MicroserviceMembership.Domain.Ports;    // Asegúrate de que este namespace sea correcto
using MicroserviceMembership.Api;

namespace MicroserviceMembership.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipsController : ControllerBase
    {
        // El controlador depende de la interfaz del repositorio
        private readonly IMembershipRepository _membershipRepository;

        // Se inyecta la dependencia a través del constructor
        public MembershipsController(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        // GET: api/memberships
        // Obtiene todas las membresías
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var memberships = await _membershipRepository.GetAllAsync();
            return Ok(memberships);
        }

        // GET: api/memberships/5
        // Obtiene una membresía por su ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var membership = await _membershipRepository.GetByIdAsync(id);
            if (membership == null)
            {
                return NotFound(); // Devuelve 404 Not Found si no existe
            }
            return Ok(membership);
        }

        // POST: api/memberships
        // Crea una nueva membresía
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Membership membership)
        {
            if (membership == null)
            {
                return BadRequest(); // Devuelve 400 Bad Request si el cuerpo está vacío
            }

            var newId = await _membershipRepository.AddAsync(membership);

            // Devuelve 201 Created con la ubicación del nuevo recurso y el objeto creado
            return CreatedAtAction(nameof(GetById), new { id = newId }, membership);
        }

        // PUT: api/memberships/5
        // Actualiza una membresía existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Membership membership)
        {
            if (id != membership.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");
            }

            var success = await _membershipRepository.UpdateAsync(membership);
            if (!success)
            {
                return NotFound(); // No se encontró el registro para actualizar
            }

            return NoContent(); // Devuelve 204 No Content, estándar para actualizaciones exitosas
        }

        // DELETE: api/memberships/5
        // Elimina una membresía por su ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _membershipRepository.DeleteAsync(id);
            if (!success)
            {
                return NotFound(); // No se encontró el registro para eliminar
            }

            return NoContent(); // Devuelve 204 No Content, estándar para eliminaciones exitosas
        }
    }
}