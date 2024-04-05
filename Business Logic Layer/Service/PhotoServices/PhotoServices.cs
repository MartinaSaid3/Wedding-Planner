using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Data_Access_Layer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Models
{
    public class PhotoServices : IPhotoServices
    {
        private readonly ApplicationEntity _context;
        private Cloudinary _cloudinary;

        public PhotoServices(ApplicationEntity dbcontext, IOptions<CloudinarySetting> config)
        {
            this._context = dbcontext;
            var Account = new Account(config.Value.CloudName, config.Value.APIKey, config.Value.APISecret);
            _cloudinary = new Cloudinary(Account);
        }

        public async Task<ImageUploadResult> AddPlaceAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")

                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);



            }
            return uploadResult;

        }
    }
}
