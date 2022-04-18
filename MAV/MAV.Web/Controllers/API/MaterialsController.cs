﻿namespace MAV.Web.Controllers.API
{
    using MAV.Common.Models;
    using MAV.Web.Data;
    using MAV.Web.Data.Repositories;
    using MAV.Web.Helpers;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class MaterialsController : Controller
    {
        private readonly IMaterialRepository materialRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IMaterialTypeRepository materialTypeRepository;
        private readonly IOwnerRepository ownerRepository;
        private readonly IUserHelper userHelper;
        private readonly DataContext _dataContext; 

        public MaterialsController(DataContext dataContext, IMaterialRepository materialRepository, IStatusRepository statusRepository, IMaterialTypeRepository materialTypeRepository,
            IOwnerRepository ownerRepository, IUserHelper userHelper)
        {
            _dataContext = dataContext;
            this.materialRepository = materialRepository;
            this.statusRepository = statusRepository;
            this.materialTypeRepository = materialTypeRepository;
            this.ownerRepository = ownerRepository;
            this.userHelper = userHelper;
        }


        [HttpGet]
        public IActionResult GetMaterials()
        {
            //return Ok(this.materialRepository.GetAll());
            //return Ok(this.materialRepository.GetMaterialWithLoansById(1));
            //return Ok(this.materialRepository.GetMaterialWithLoans());
            return Ok(this.materialRepository.GetAllMaterialsWithTypeWithStatusAndOwner());
            //return Ok(this.materialRepository.GetMaterialWithTypeWithStatusAndOwnerById(1));
            //return Ok(this.materialRepository.GetMaterialWithTypeAndStatusBySerialNum("6817654"));
            //return Ok(this.materialRepository.GetMaterialBySerialNum("897654"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByBrand("Sony"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByStatus("Disponible"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByType("Cable"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByName("VGA"));
            //return Ok(this.materialRepository.GetMaterialsWithTypeWithStatusAndOwnerByLabel("MAV01"));
        }


        [HttpPost]
        public async Task<IActionResult> PostMaterial([FromBody] MAV.Common.Models.MaterialRequest material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var status = this.statusRepository.GetStatusByName(material.Status);
            //if (status == null)
            //{
            //    return BadRequest(ModelState);
            //}

            var owner = this.ownerRepository.GetByIdOwnerWithMaterials(material.Owner);

            if (owner == null)
            {
                return BadRequest("Not valid owner.");
            }

            var status = await _dataContext.Statuses.FindAsync(material.Status);
            if (status == null)
            {
                return BadRequest("Not valid status");
            }
            var materialType = await _dataContext.MaterialTypes.FindAsync(material.MaterialType);
            if (materialType == null)
            {
                return BadRequest("Not valid material type");
            }

            //var materialType = this.materialTypeRepository.GetMaterialTypesByName(material.MaterialType);
            //if (materialType == null)
            //{
            //    return BadRequest(ModelState);
            //}
            //var owner = this.ownerRepository.GetGoodOwnerWithEmail(material.Owner);          
            //if (owner == null)
            //{
            //    return BadRequest(ModelState);
            //}

            var imageUrl = string.Empty;
            if (material.ImageArray != null && material.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(material.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\Images\\Materiales";
                var fullPath = $"~/Images/Materiales/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                }
            }

            var entityMaterial = new MAV.Web.Data.Entities.Material
            {

                Name = material.Name,
                Label = material.Label,
                Brand = material.Brand,
                MaterialModel = material.MaterialModel,
                SerialNum = material.SerialNum,
                Function = material.Function,
                Owner = owner,
                Status = status,
                MaterialType = materialType
                //Owner = owner
                //Status = status,
                //ImageURL = imageUrl                
            };
            //if (entityMaterial == null)
            //{
            //    return BadRequest("entityMaterial not found");
            //}
            //var newMaterial = await this.materialRepository.CreateAsync(entityMaterial);
            _dataContext.Materials.Add(entityMaterial);
            await _dataContext.SaveChangesAsync();
            return Ok(materialRepository.ToMaterialResponse(entityMaterial));
            //return Ok(newMaterial); 
        }


        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMaterial([FromRoute] int id, [FromBody] MAV.Common.Models.MaterialRequest material)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (id != material.Id)
        //    {
        //        return BadRequest();
        //    }
        //    var oldMaterial = await this.materialRepository.GetByIdAsync(id);
        //    if (oldMaterial == null)
        //    {
        //        return BadRequest("Id not found");
        //    }
        //    var status = this.statusRepository.GetStatusByName(material.Status);
        //    if (status == null)
        //    {
        //        return BadRequest("status not found");
        //    }
        //    var materialType = this.materialTypeRepository.GetMaterialTypesByName(material.MaterialType);
        //    if (materialType == null)
        //    {
        //        return BadRequest("materialtype not found");
        //    }
        //    var owner = this.ownerRepository.GetGoodOwnerWithEmail(material.Owner);
        //    if (owner == null)
        //    {
        //        return BadRequest("owner not found");
        //    }
        //    oldMaterial.Name = material.Name;
        //    oldMaterial.Owner = owner;
        //    oldMaterial.Label = material.Label;
        //    oldMaterial.Brand = material.Brand;
        //    oldMaterial.MaterialModel = material.MaterialModel;
        //    oldMaterial.SerialNum = material.SerialNum;
        //    oldMaterial.Status = status;
        //    oldMaterial.MaterialType = materialType;
        //    var updateMaterial = await this.materialRepository.UpdateAsync(oldMaterial);
        //    return Ok(updateMaterial);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var oldMaterial = await this.materialRepository.GetByIdAsync(id);
            if (oldMaterial == null)
            {
                return BadRequest("Id not found");
            }
            await this.materialRepository.DeleteAsync(oldMaterial);
            return Ok(oldMaterial);
        }
    }
}
