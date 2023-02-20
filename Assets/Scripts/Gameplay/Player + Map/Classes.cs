using UnityEngine;
using System.Collections.Generic;
public class Classes
{
    public class hasYield
    {
        float gold;
        float magicka;
        float science;
        float influence;
        float festivity;
        #region
        public float GetGold()
        {
            return gold;
        }
        public void AddGold(float f)
        {
            gold += f;
        }
        public float GetMagicka()
        {
            return magicka;
        }
        public void AddMagicka(float f)
        {
            magicka += f;
        }
        public float GetScience()
        {
            return science;
        }
        public void AddScience(float f)
        {
            science += f;
        }
        public float GetInfluence()
        {
            return influence;
        }
        public void AddInfluence(float f)
        {
            influence += f;
        }
        public float GetFestivity()
        {
            return festivity;
        }
        public void AddFestivity(float f)
        {
            festivity += f;
        }
        #endregion
    }

    public class City : hasYield
    {
        float growth;
        float production;
        string name;

        public City(string _name)
        {
            name = _name;
        }

        public float GetGrowth()
        {
            return growth;
        }
        public void AddGrowth(float f)
        {
            growth += f;
        }
        public float GetProduction()
        {
            return production;
        }
        public void AddProduction(float f)
        {
            production += f;
        }
        public string GetYield(int i)
        {
            if (i == 0)
            {
                return GetGold().ToString();
            }
            if (i == 1)
            {
                return GetMagicka().ToString();
            }
            if (i == 2)
            {
                return GetProduction().ToString();
            }
            if (i == 3)
            {
                return GetScience().ToString();
            }
            if (i == 4)
            {
                return GetInfluence().ToString();
            }
            if (i == 5)
            {
                return GetFestivity().ToString();
            }
            else
            {
                return GetGrowth().ToString();
            }
        }
    }

    public class Unit
    {
        
    }

    public class MilitaryUnit : Unit
    {

    }

    public class PassiveUnity : Unit
    {

    }

    public class Player : hasYield
    {
        bool isHuman = false;
        List<City> cities = new List<City>();
        public void setHuman()
        {
            isHuman = true;
        }
        public bool getHuman()
        {
            return isHuman;
        }
        public void settleCity(string name)
        {
            cities.Add(new City(name));
        }
        public int getNumberCities()
        {
            return cities.Count;
        }
        public void removeCity()
        {
            cities.Remove(new City(""));
        }
        public City GetCity(int i)
        {
            return cities[i];
        }
        public List<City> GetCities()
        {
            return cities;
        }
        public void getIncome()
        {
            foreach(City city in cities)
            {
                AddGold(city.GetGold());
                AddMagicka(city.GetMagicka());
                city.AddGrowth(city.GetGrowth());
                city.AddProduction(city.GetProduction());
                AddScience(city.GetScience());
                AddInfluence(city.GetInfluence());
                city.AddInfluence(city.GetInfluence());
                AddFestivity(city.GetFestivity());
                city.AddFestivity(city.GetFestivity());
            }
        }
    }
}
