using Application.ViewModels.ImageDTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.DiamondDTOs
{
    public class DiamondDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Origin { get; set; }
        public float CaratWeight { get; set; }
        public string Clarity { get; set; }
        public string Cut { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Quantity {  get; set; }
        public ICollection<ImageDTO> Images { get; set; } = new List<ImageDTO>();
        public bool IsDeleted { get; set; }
    }
}

