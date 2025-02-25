﻿using Covid19.Helper;
using Covid19.Models.IManagers;
using Covid19.Models.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models.Managers
{
    public class CountriesDeathsAndSickPerMillionBySickManager : ICountriesDeathsAndSickPerMillionBySickManager
    {
        private MySqlDB mySqlDB;

        public CountriesDeathsAndSickPerMillionBySickManager(MySqlDB db)
        {
            mySqlDB = db;
        }

        public IEnumerable<CountriesDeathsAndSickPerMillionBySick> GetBySick(string orderBy, string date)
        { // yuval changed on 24.12
            List<object[]> listOfAvg = mySqlDB.GetSqlListWithoutParameters("select distinct Country, ( Cumulative_deaths *1000 / PopTotal) deathPerMillion, ( Cumulative_cases / PopTotal) sickPerMillion " +
                "from " +
                "(select distinct * " +
                "from who_covid_19_global_data " +
                "where Date_reported = '"+ date + "') sick " +
                "inner join " +
                "(select distinct * " +
                "from population_worldwide " +
                "where time = 2020) density " +
                "on sick.Country = density.Location " +
                "order by sick.Cumulative_deaths " + orderBy);
            return GlobalFunction.ConvertListObjectByGeneric<CountriesDeathsAndSickPerMillionBySick>(listOfAvg, ConvertObjectCountriesDeathsAndSickPreMillionBySick);
        }
        public static CountriesDeathsAndSickPerMillionBySick ConvertObjectCountriesDeathsAndSickPreMillionBySick(object[] infoFromDB)
        {
            try
            {
                return new CountriesDeathsAndSickPerMillionBySick
                {
                    Country = infoFromDB[0].ToString(),
                    DeathPerMillion = Convert.ToDouble(infoFromDB[1].ToString()),
                    SickPerMillion = Convert.ToDouble(infoFromDB[2].ToString())
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    
}
