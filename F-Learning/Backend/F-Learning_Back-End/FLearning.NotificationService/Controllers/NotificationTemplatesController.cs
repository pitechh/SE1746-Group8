using FLearning.NotificationService.Models;
using FLearning.NotificationService.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FLearning.NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationTemplatesController : ControllerBase
    {
        private readonly INotificationRepository _repository;

        public NotificationTemplatesController(INotificationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTemplates()
        {
            var templates = await _repository.GetAllTemplatesAsync();
            return Ok(templates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemplate(int id)
        {
            var template = await _repository.GetTemplateByIdAsync(id);
            if (template == null)
                return NotFound();

            return Ok(template);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTemplate([FromBody] NotificationTemplate template)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _repository.CreateTemplateAsync(template);
            return CreatedAtAction(nameof(GetTemplate), new { id }, template);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemplate(int id, [FromBody] NotificationTemplate template)
        {
            if (id != template.Id)
                return BadRequest("ID mismatch");

            var existingTemplate = await _repository.GetTemplateByIdAsync(id);
            if (existingTemplate == null)
                return NotFound();

            template.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateTemplateAsync(template);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            var template = await _repository.GetTemplateByIdAsync(id);
            if (template == null)
                return NotFound();

            await _repository.DeleteTemplateAsync(id);
            return NoContent();
        }
    }
}
