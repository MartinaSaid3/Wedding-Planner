using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public interface IPhotoServices
    {
        Task<ImageUploadResult> AddPlaceAsync(IFormFile file);
    }
}
