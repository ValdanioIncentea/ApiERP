using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ApiLocal
{
    public class Helper
    {
		public static string Abreviar(string Nome)
		{

			String[] Nomes = Nome.Split(' ');

			string Abreviatura = String.Empty;

			for (int i = 0; i < Nomes.Length; i++)
			{

				if (Nomes[i].Length > 2 && Nomes[i] != "Lda." && Nomes[i] != "S.A.")
					Abreviatura = Abreviatura + (Nomes[i].Substring(0, 1).ToUpper());

			}

			return Abreviatura;

		}
	}
}