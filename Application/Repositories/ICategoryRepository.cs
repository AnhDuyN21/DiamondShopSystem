﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
	public interface ICategoryRepository : IGenericRepository<Category>
	{
		Task<bool> NameIsExisted(string name, float size, float length);
	}
}