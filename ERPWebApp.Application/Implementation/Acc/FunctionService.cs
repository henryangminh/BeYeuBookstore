using AutoMapper.QueryableExtensions;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels.Acc;
using BeYeuBookstore.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BeYeuBookstore.Data.Entities;
using AutoMapper;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Data.Enums;

namespace BeYeuBookstore.Application.Implementation.Acc
{
    public class FunctionService : IFunctionService
    {
        private IRepository<Function,string> _functionRepository;
        private IRepository<Permission, int> _permissionRepository;
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FunctionService(IRepository<Function, string> functionRepository, IRepository<Permission, int> permissionRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void Add(FunctionViewModel functionVm)
        {
            var function = _mapper.Map<Function>(functionVm);
            _functionRepository.Add(function);
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _functionRepository.FindSingle(x => x.KeyId == id);
            return Mapper.Map<Function, FunctionViewModel>(function);
        }

        public Task<List<FunctionViewModel>> GetAll(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter)) query = query.Where(x => x.Name.Contains(filter));

			//query = query.OrderBy(x => x.SortOrder).ThenBy(x => x.ParentId);
			//var data = new List<FunctionViewModel>();
			//foreach (var item in query)
			//{
			//	var _data = Mapper.Map<Function, FunctionViewModel>(item);
			//	data.Add(_data);
			//}
			//return data;
			return query.OrderBy(x => x.SortOrder).ThenBy(x => x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();
		}
        /// <summary>
        ///  // viết 1 hàm truyền vào list role trả về những function nào được truy xuất từ roles đó.
        /// </summary>
        /// <param name="roles">role là 1 chuỗi các nhóm quyền cách nhau bằng dấu ;</param>
        /// <returns></returns>
        public List<FunctionViewModel> GetAllByRoles(string roles)
        {
            var listrole = roles.Split(";");
            var query = _permissionRepository.FindAll(p=>listrole.Contains(p.AppRole.Name)&&p.CanRead ==true,p=>p.Function).ToList();
            var listFunction = _functionRepository.FindAll().ToList();
            var data = new List<FunctionViewModel>();
            var listparent = new List<FunctionViewModel>();
            foreach(var item in query)
            {
                var temp = Mapper.Map<Function, FunctionViewModel>(item.Function);
                if(!data.Contains(temp))
                    data.Add(temp);
            }
            for (int i = 0; i < listFunction.Count(); i++)
            {
                for (int j = 0; j < data.Count(); j++)
                {
                    if (listFunction[i].KeyId == data[j].ParentId)
                    {
                        if (data.Count(p => p.KeyId == listFunction[i].KeyId) == 0 && listparent.Count(p => p.KeyId == listFunction[i].KeyId) == 0)
                        {
                            listparent.Add(Mapper.Map<Function, FunctionViewModel>(listFunction[i]));
                            break;
                        }

                    }
                }
            }
            data.AddRange(listparent);
            data = data.OrderBy(x => x.SortOrder).ThenBy(x => x.ParentId).ToList();
            return data;
        }
        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            return _functionRepository.FindAll(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>();
        }
        public bool Save()
        {
            return _unitOfWork.Commit();
        }

        public void Update(FunctionViewModel functionVm)
        {

            var functionDb = _functionRepository.FindById(functionVm.KeyId);
            var function = _mapper.Map<Function>(functionVm);
        }

        public void ReOrder(string sourceId, string targetId)
        {

            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _functionRepository.Update(source);
            _functionRepository.Update(target);

        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //Get all sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.KeyId));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.KeyId];
                _functionRepository.Update(child);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        
    }
}
