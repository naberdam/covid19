﻿using Covid19.Helper;
using Covid19.Models.IManagers;
using Covid19.Models.ObjectClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19.Models.Managers
{
    public class CountrySickAndDeathsPerMillionAndGrowthManager : ICountrySickAndDeathsPerMillionAndGrowthManager
    {
        private MySqlDB mySqlDB;

        public CountrySickAndDeathsPerMillionAndGrowthManager(MySqlDB db)
        {
            mySqlDB = db;
        }
        /*public IEnumerable<CountrySickAndDeathsOrDensity> GetCountriesWithDensityWithSickAndDeathsPerMillion(string orderBy, string date)
        { // i changed it 24.12 yuval
            List<object[]> list = mySqlDB.GetSqlListWithoutParameters("select distinct Country, PopDensity, Cumulative_cases, Cumulative_deaths,( Cumulative_deaths * 1000 / PopTotal) deathPerMillion, ( Cumulative_cases / PopTotal) sickPerMillion, PopTotal " +
                "from (select distinct * " +
                "from who_covid_19_global_data " +
                "where Date_reported = '" + date + "') sick " +
                "inner join " +
                "(select distinct * " +
                "from population_worldwide " +
                "where time = 2020) " +
                "density on sick.Country = density.Location " +
                "order by sick.Cumulative_deaths " + orderBy);
            return GlobalFunction.ConvertListObjectByGeneric<CountrySickAndDeathsOrDensity>(list, ConvertObjectCountrySickAndDeathsOrDensity);
        }*/

        public IEnumerable<CountrySickAndDeathsPerMillionAndGrowth> GetCountryDeathsAndSickPerMillionAndGrowthOrderByDeaths(string orderBy, string date, int numYears)
        {
            string countingFromYear = (2020 - numYears).ToString();
            List<object[]> listOfDeaths = mySqlDB.GetSqlListWithoutParameters("select Country, growth, ( Cumulative_deaths*1000 / PopTotal) deathPerMillion, ( Cumulative_cases*1000 / PopTotal) sickPerMillion " +
                "from " +
                "(select distinct * from who_covid_19_global_data where Date_reported = '" + date + "') sick " +
                "inner join (select distinct t1.Location,  t1.PopTotal/ t2.PopTotal as growth, t1.PopDensity, t1.PopTotal PopTotal " +
                "from population_worldwide t1 ,population_worldwide t2 " +
                "where t1.Location = t2.Location and t1.Time = '2020' and t2.Time = " + countingFromYear + ") " +
                "density on sick.Country = density.Location order by deathPerMillion " + orderBy);
            return GlobalFunction.ConvertListObjectByGeneric<CountrySickAndDeathsPerMillionAndGrowth>(listOfDeaths, ConvertObjectCountrySickAndDeathsPerMillionAndGrowth);
        }

        public IEnumerable<CountrySickAndDeathsPerMillionAndGrowth> GetCountryDeathsAndSickPerMillionAndGrowthOrderByGrowth(string orderBy, string date, int numYears)
        {
            string countingFromYear = (2020 - numYears).ToString();
            List<object[]> listOfGrowth = mySqlDB.GetSqlListWithoutParameters("select Country, growth, ( Cumulative_deaths*1000 / PopTotal) deathPerMillion, ( Cumulative_cases*1000 / PopTotal) sickPerMillion " +
                "from " +
                "(select distinct * from who_covid_19_global_data where Date_reported = '" + date + "') sick " +
                "inner join (select distinct t1.Location,  t1.PopTotal/ t2.PopTotal as growth, t1.PopDensity, t1.PopTotal PopTotal " +
                "from population_worldwide t1 ,population_worldwide t2 " +
                "where t1.Location = t2.Location and t1.Time = '2020' and t2.Time = " + countingFromYear + ") " +
                "density on sick.Country = density.Location order by growth " + orderBy);
            return GlobalFunction.ConvertListObjectByGeneric<CountrySickAndDeathsPerMillionAndGrowth>(listOfGrowth, ConvertObjectCountrySickAndDeathsPerMillionAndGrowth);
        }

        public IEnumerable<CountrySickAndDeathsPerMillionAndGrowth> GetCountryDeathsAndSickPerMillionAndGrowthOrderBySick(string orderBy, string date, int numYears)
        {
            string countingFromYear = (2020 - numYears).ToString();
            List<object[]> listOfSick = mySqlDB.GetSqlListWithoutParameters("select Country, growth, ( Cumulative_deaths*1000 / PopTotal) deathPerMillion, ( Cumulative_cases*1000 / PopTotal) sickPerMillion " +
                "from " +
                "(select distinct * from who_covid_19_global_data where Date_reported = '" + date + "') sick " +
                "inner join (select distinct t1.Location,  t1.PopTotal/ t2.PopTotal as growth, t1.PopDensity, t1.PopTotal PopTotal " +
                "from population_worldwide t1 ,population_worldwide t2 " +
                "where t1.Location = t2.Location and t1.Time = '2020' and t2.Time = " + countingFromYear + ") " +
                "density on sick.Country = density.Location order by sickPerMillion " + orderBy);
            return GlobalFunction.ConvertListObjectByGeneric<CountrySickAndDeathsPerMillionAndGrowth>(listOfSick, ConvertObjectCountrySickAndDeathsPerMillionAndGrowth);
        }

        public static CountrySickAndDeathsPerMillionAndGrowth ConvertObjectCountrySickAndDeathsPerMillionAndGrowth(object[] infoFromDB)
        {
            try
            {
                return new CountrySickAndDeathsPerMillionAndGrowth
                {
                    Country = infoFromDB[0].ToString(),
                    Growth = Convert.ToDouble(infoFromDB[1].ToString()),
                    DeathPerMillion = Convert.ToDouble(infoFromDB[2].ToString()),
                    SickPerMillion = Convert.ToDouble(infoFromDB[3].ToString())
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
