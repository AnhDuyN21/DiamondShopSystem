﻿using Application.ViewModels.AccountDTOs;
using Application.ViewModels.DiamondDTOs;
using Application.ViewModels.ImageDTOs;
using Application.ViewModels.ProductDTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Mappers
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //Account
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Account, CreateAccountDTO>().ReverseMap();
            CreateMap<Account, UpdateAccountDTO>().ReverseMap();
            CreateMap<Account, RegisterAccountDTO>().ReverseMap();
			//Diamond
			CreateMap<Diamond, DiamondDTO>().ReverseMap();
            CreateMap<Diamond, UpdateDiamondDTO>().ReverseMap();
            CreateMap<Diamond, CreateDiamondDTO>().ReverseMap();
            //Product
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
            //Image
            CreateMap<Image, ImageDTO>().ReverseMap();





		}
    }
}
