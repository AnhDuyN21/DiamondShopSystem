﻿using Application;
using Application.Interfaces;
using Application.Repositories;
using Infrastructures.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private ICurrentTime _currentTime;
        private IClaimsService _claimsService;

        public UnitOfWork(AppDbContext dbContext, ICurrentTime currentTime, IClaimsService claimsService)
        {
            _dbContext = dbContext;
            _currentTime = currentTime;
            _claimsService = claimsService;
        }

        private IDiamondRepository _diamondRepository;
        public IDiamondRepository DiamondRepository
        {
            get
            {
                if (_diamondRepository is null)
                {
                    _diamondRepository = new DiamondRepository(_dbContext, _currentTime, _claimsService);
                }
                return _diamondRepository;
            }
        }

        private IAccountRepository _accountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository is null)
                {
                    _accountRepository = new AccountRepository(_dbContext, _currentTime, _claimsService);
                }
                return _accountRepository;
            }
        }

        private IOrderRepository _orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository is null)
                {
                    _orderRepository = new OrderRepository(_dbContext, _currentTime, _claimsService);
                }
                return _orderRepository;
            }
        }
        private IImageRepository _imageRepository;
        public IImageRepository ImageRepository
        {
            get
            {
                if (_imageRepository is null)
                {
                    _imageRepository = new ImageRepository(_dbContext, _currentTime, _claimsService);
                }
                return _imageRepository;
            }
        }
        private IPaymentRepository _paymentRepository;
        public IPaymentRepository PaymentRepository
        {
            get
            {
                if (_paymentRepository is null)
                {
                    _paymentRepository = new PaymentRepository(_dbContext);
                }
                return _paymentRepository;
            }
        }

		private IProductRepository _productRepository;
		public IProductRepository ProductRepository
		{
			get
			{
				if (_productRepository is null)
				{
					_productRepository = new ProductRepository(_dbContext, _currentTime, _claimsService);
				}
				return _productRepository;
			}
		}
        private ICartRepository _cartRepository;
        public ICartRepository CartRepository
        {
            get
            {
                if (_cartRepository is null)
                {
                    _cartRepository = new CartRepository(_dbContext,_currentTime,_claimsService);
                }
                return _cartRepository;
            }
        }

        private IProductDiamondRepository _productDiamondRepository;
		public IProductDiamondRepository ProductDiamondRepository
		{
			get
			{
				if (_productDiamondRepository is null)
				{
					_productDiamondRepository = new ProductDiamondRepository(_dbContext, _currentTime, _claimsService);
				}
				return _productDiamondRepository;
			}
		}

        private ICategoryRepository _categoryRepository;
		public ICategoryRepository CategoryRepository
		{
			get
			{
				if (_categoryRepository is null)
				{
					_categoryRepository = new CategoryRepository(_dbContext, _currentTime, _claimsService);
				}
				return _categoryRepository;
			}
		}

        private IRoleRepository _roleRepository; 
        public IRoleRepository RoleRepository
        {
            get
            {
                if (_roleRepository is null)
                {
                    _roleRepository = new RoleRepository(_dbContext);
                }
                return (_roleRepository);
            }
        }

		private IProductTypeRepository _productTypeRepository;
		public IProductTypeRepository ProductTypeRepository
		{
			get
			{
				if (_productTypeRepository is null)
				{
					_productTypeRepository = new ProductTypeRepository(_dbContext, _currentTime, _claimsService);
				}
				return (_productTypeRepository);
			}
		}
		private IProductWarrantyRepository _productWarrantyRepository;
		public IProductWarrantyRepository ProductWarrantyRepository
		{
			get
			{
				if (_productWarrantyRepository is null)
				{
					_productWarrantyRepository = new ProductWarrantyRepository(_dbContext, _currentTime, _claimsService);
				}
				return (_productWarrantyRepository);
			}
		}
		private IPromotionRepository _promotionRepository;
        public IPromotionRepository PromotionRepository
        {
            get
            {
                if (_promotionRepository is null)
                {
                    _promotionRepository = new PromotionRepository(_dbContext, _currentTime, _claimsService);
                }
                return _promotionRepository;
            }
        }


        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
