using Microsoft.AspNetCore.Mvc;
using ProyectoInventarioAPI.Models;
using ProyectoInventarioAPI.Services;

namespace ProyectoInventarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return Ok(await _usuarioService.ObtenerTodos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPorId(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                var nuevo = await _usuarioService.RegistrarUsuario(usuario);
                return CreatedAtAction("GetUsuario", new { id = nuevo.UsuarioId }, nuevo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login([FromBody] LoginDto login)
        {
            var usuario = await _usuarioService.ValidarCredenciales(login.Email, login.Password);

            if (usuario == null)
                return Unauthorized("Credenciales incorrectas.");

            return Ok(usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId) return BadRequest();
            await _usuarioService.ActualizarUsuario(usuario);
            return NoContent();
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}