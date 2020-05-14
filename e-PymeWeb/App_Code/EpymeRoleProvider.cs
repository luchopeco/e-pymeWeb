using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Negocio;
using Entidades;


/// <summary>
/// SicoRoleProvider: Custom Role Provider
/// </summary>
public class EpymeRoleProvider : RoleProvider
{
    string pApplicationName;

    public EpymeRoleProvider()
    {
    }

    public override string ApplicationName
    {
        get { return pApplicationName; }
        set { pApplicationName = value; }
    }

    public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
    {
        Console.WriteLine("Hola: Iniciado");
        if (config == null)
            throw new ArgumentNullException("config");

        if (name == null || name.Length == 0)
            name = "EpymeRoleProvider";

        if (String.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "Epyme Role Provider");
        }

        // Initialize the abstract base class.
        base.Initialize(name, config);


        if (config["applicationName"] == null || config["applicationName"].Trim() == "")
        {
            pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
        }
        else
        {
            pApplicationName = config["applicationName"];
        }
    }

    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
        throw new NotImplementedException();
    }

    public override string[] GetAllRoles()
    {
        // Devuelvo todas las paginas disponibles (Su código).
        using (ControladorMenus mnu = new ControladorMenus())
        {
            //Console.WriteLine("Hola 1");
            List<Pagina> pags = mnu.BuscarListPaginas();
            return pags.Select(p => p.Codigo).ToArray();
        }

    }

    public override string[] GetRolesForUser(string username)
    {
        try
        {
            using (ControladorMenus mnu = new ControladorMenus())
            {
                //Console.WriteLine("Hola 2");
                List<Pagina> pags = mnu.BuscarListPaginasXUsuario(Convert.ToInt32(username));
                return pags.Select(p => p.Codigo).ToArray();
            }
        }
        catch (ExcepcionPropia)
        {
            string[] s = new string[0];
            return s;
        }
        


    }

    public override string[] GetUsersInRole(string roleName)
    {
        throw new NotImplementedException();
    }

    public override bool IsUserInRole(string username, string roleName)
    {
        //Console.WriteLine("Hola 3");
        string[] roles = GetRolesForUser(username);
        return GetRolesForUser(username).Where(x => x == roleName).First().Length > 0;

    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public override bool RoleExists(string roleName)
    {
        return GetAllRoles().Where(x => x == roleName).First().Length > 0;
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
        throw new NotImplementedException();
    }

    public override void CreateRole(string roleName)
    {
        throw new NotImplementedException();
    }


}