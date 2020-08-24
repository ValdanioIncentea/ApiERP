using ErpBS100;
using PortalApi.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PortalApi.App_Start
{
    public class AssemblyResolve
    {
        const string PRIMAVERA_FILES_FOLDER = "PRIMAVERA\\SG100\\Apl";

        public AssemblyResolve()
        {
            Resolve();
        }
        public static void Resolve() => AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

        static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            System.Reflection.AssemblyName assemblyName = new System.Reflection.AssemblyName(args.Name);
            string assemblyFullName = assemblyFullName = System.IO.Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), PRIMAVERA_FILES_FOLDER), assemblyName.Name + ".dll");

            if (System.IO.File.Exists(assemblyFullName))
                return System.Reflection.Assembly.LoadFile(assemblyFullName);
            else return null;

        }

    }
}