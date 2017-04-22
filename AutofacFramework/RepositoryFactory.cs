using AutofacFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutofacFramework
{
    #region Repostory

    public interface IBase { }
    public interface IRepository<TEntity> where TEntity : ViewModel
    {
        IEnumerable<TEntity> AllItems { get; }
        int Add(TEntity item);
        TEntity GetById(int id);
        bool TryDelete(int id);
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : ViewModel
    {
        public Repository() { }
        readonly List<TEntity> _items = new List<TEntity>();

        public IEnumerable<TEntity> AllItems
        {
            get { return _items; }
        }

        public TEntity GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public int Add(TEntity item)
        {
            item.Id = 1 + _items.Max(x => (int?)x.Id) ?? 1;
            _items.Add(item);
            return item.Id;
        }

        public bool TryDelete(int id)
        {
            var item = GetById(id);

            if (item == null) { return false; }

            _items.Remove(item);

            return true;
        }
    }
    #endregion

    #region Model
    public interface IViewModel
    { }
    public class ViewModel : IViewModel
    {
        public int Id { get; set; }

    }

    public class Menu : ViewModel
    {
        public string M_Name { get; set; }
    }

    public class Function : ViewModel
    {
        public int M_Id { get; set; }
        public string F_Name { get; set; }
    }

    public class AuthItem : ViewModel
    {
        public string MenuName { get; set; }
        public List<Function> Functions { get; set; }
    }
    #endregion

    #region Dal
    public interface IMenuRepository : IRepository<Menu>
    {
        List<Menu> GetAllMenus();
    }

    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public List<Menu> GetAllMenus()
        {
            return AllItems.ToList();
        }
    }

    public interface IFunctionRepository : IRepository<Function>
    {
        List<Function> GetAllFunctions();
    }

    public class FunctionRepository : Repository<Function>, IFunctionRepository
    {
        public List<Function> GetAllFunctions()
        {
            return AllItems.ToList();
        }
    }

    #endregion

    #region Service

    public interface IMenuService
    {

        List<Menu> GetAllMenu();
    }

    public interface IAuthService:IRepository<AuthItem>
    {
        bool Auth();

        List<AuthItem> GetAuthItems();
    }

    public class MenuService : IMenuService
    {
        private IMenuRepository _menuRepository;
        public MenuService(IMenuRepository menuRepository) : base()
        {
            _menuRepository = menuRepository;
        }
        public List<Menu> GetAllMenu()
        {
            return _menuRepository.GetAllMenus();
        }
    }

    public class AuthService : Repository<AuthItem>, IAuthService
    {
        private IMenuRepository _menuRepository;
        private IFunctionRepository _functionRepository;
        public AuthService(IMenuRepository menuRepository, IFunctionRepository functionRepository)
        {
            _menuRepository = menuRepository;
            _functionRepository = functionRepository;
        }

        public bool Auth()
        {
            var menuId = _menuRepository.Add(new Menu { M_Name = "UserManger" });

            var functionId = _functionRepository.Add(new Function { M_Id = menuId, F_Name = "Add" });
            var functionId2 = _functionRepository.Add(new Function { M_Id = menuId, F_Name = "Modify" });

            return (menuId > 0 && functionId > 0 && functionId2 > 0);
        }

        public List<AuthItem> GetAuthItems()
        {
            var result = new List<AuthItem>();
            var functions = new List<Function>();

            _menuRepository.AllItems.ToList().ForEach(m =>
            {
                var authMenu = new AuthItem { MenuName = m.M_Name };
                authMenu.Id = 1 + result.Max(x => (int?)x.Id) ?? 1;
                result.Add(authMenu);
                _functionRepository.AllItems.Where(f => f.M_Id == m.Id).ToList().ForEach(f =>
                  {
                      functions.Add(f);
                      authMenu.Functions = functions;
                  });
            });
            return result;
        }
    }
    #endregion
}
