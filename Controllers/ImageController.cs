// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Data.SqlClient;
// using NetDapperWebApi.Common.Interfaces;
// using NetDapperWebApi.DTO.Creates;
// using NetDapperWebApi.DTO.Updates;

// namespace NetDapperWebApi.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ImageController : ControllerBase
//     {
//         private readonly IImageService _imageService;

//         public ImageController(IImageService imageService)
//         {
//             _imageService = imageService;
//         }

//         [HttpPost]
//         public async Task<IActionResult> InsertImage([FromForm] ImageCreateDTO imageDto)
//         {
//             var inserted = await _imageService.UploadImagesAsync(imageDto);
//             return inserted != null ? Ok(new { inserted }) : BadRequest();
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetImagesByEntity(string entityType, int entityId)
//         {
//             var images = await _imageService.GetImagesByEntityAsync(entityType, entityId);
//             return Ok(images);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateImage(int id, [FromBody] ImageUpdateDTO imageUpdateDto)
//         {
//             var updated = await _imageService.UpdateImageAsync(id, imageUpdateDto);
//             if (updated == null) return NotFound();
//             return Ok(new { updated });
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteImage(int id)
//         {
//             var deleted = await _imageService.DeleteImageAsync(id);
//             return deleted ? NoContent() : NotFound();
//         }

//         [HttpPost("bulk-insert")]
//         public async Task<IActionResult> BulkInsertImages([FromForm] CreateMutipleImage images)
//         {
//             try
//             {
//                 var success = await _imageService.BulkInsertImagesAsync(images);
//                 return Ok(new { success });
//             }
       
//             catch (System.Exception ex)
//             {
//                 return BadRequest(new { ex.Message });
//             }
//         }

//         // [HttpPost("bulk-delete")]
//         // public async Task<IActionResult> BulkDeleteImages([FromBody] IEnumerable<int> ids)
//         // {
//         //     var success = await _imageService.BulkDeleteImagesAsync(ids);
//         //     return success ? Ok() : BadRequest();
//         // }
//     }
// }