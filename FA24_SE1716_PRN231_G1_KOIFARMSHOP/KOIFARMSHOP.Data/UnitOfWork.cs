using KOIFARMSHOP.Data.Models;
using KOIFARMSHOP.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KOIFARMSHOP.Data
{
    public class UnitOfWork
    {
        private FA24_SE1716_PRN231_G1_KOIFARMSHOPContext context;
        private AnimalRepository animalRepository;
        private AnimalImageRepository animalImageRepository;
        private CategoryRepository categoryRepository;
        private ConsignmentRepository consignmentRepository;
        private CustomerRepository customerRepository;
        private OrderRepository orderRepository;
        private OrderDetailRepository orderDetailRepository;
        private OrderPromotionRepository orderPromotionRepository;
        private PaymentRepository paymentRepository;
        private ProductRepository productRepository;
        private ProductImageRepository productImageRepository;
        private PromotionRepository promotionRepository;
        private StaffRepository staffRepository;

        public UnitOfWork() { 
        context ??= new FA24_SE1716_PRN231_G1_KOIFARMSHOPContext();
        }

        public AnimalRepository AnimalRepository
        {
        get { return animalRepository ??= new AnimalRepository(context); } 
        }
        public AnimalImageRepository AnimalImageRepository
        {
            get { return animalImageRepository ??= new AnimalImageRepository(context); }
        }
        public CategoryRepository CategoryRepository
        {
            get { return categoryRepository ??= new CategoryRepository(context); }
        }
        public ConsignmentRepository ConsignmentRepository
        {
            get { return consignmentRepository ??= new ConsignmentRepository(context); }
        }
        public CustomerRepository CustomerRepository
        {
            get { return customerRepository ??= new CustomerRepository(context); }

        }
        public OrderRepository OrderRepository
        {
            get { return orderRepository ??= new OrderRepository(context); }
        }
        private OrderDetailRepository OrderDetailRepository
        {
            get { return orderDetailRepository ??= new OrderDetailRepository(context); }
        }
        private OrderPromotionRepository OrderPromotionRepository
        {
            get { return orderPromotionRepository ??= new OrderPromotionRepository(context); }
        }
        private PaymentRepository PaymentRepository
        {
            get { return paymentRepository ??= new PaymentRepository(context); }
        }
        public ProductRepository ProductRepository
        {
           get { return productRepository ??= new ProductRepository(context); }
        }
        public ProductImageRepository ProductImageRepository
        {
            get { return productImageRepository ??= new ProductImageRepository(context); }
        }
        private PromotionRepository PromotionRepository
        {
            get { return promotionRepository ??= new PromotionRepository(context); }
        }
        public StaffRepository StaffRepository
        {
            get { return staffRepository ??= new StaffRepository(context); }
        }


    }
}
