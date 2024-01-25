using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Classes contains all of my most important classes, such as Player and Unit
/// </summary>
public class Classes
{
    /// <summary>
    /// Things which the city can produce
    /// </summary>
    public class Produce
    {
        public Sprite m_Sprite; // The sprite associated with the product item
        public int m_goldInc; // The amount of gold income the product item provides
        public int m_magicInc; // The amount of magic income the product item provides
        public int m_prodInc; //The amount of production income the product item provides
        public int m_sciInc; // The amount of science income the product item provides
        public int m_infInc; // The amount of influence income the product item provides
        public int m_festInc; // The amount of festivity income the product item provides
        public int m_groInc; // The amount of growth income the product item provides
        public float m_goldMult; // The multiplier applied to gold income provided by the product
        public float m_magicMult; // The multiplier applied to magic income provided by the product
        public float m_prodMult; // The multiplier applied to production income provided by the product
        public float m_sciMult; // The multiplier applied to science income provided by the product
        public float m_infMult; // The multiplier applied to influence income provided by the product
        public float m_festMult; // The multiplier applied to festivity income provided by the product
        public float m_groMult; // The multiplier applied to growth income provided by the product
        /// <summary>
        /// This is a constructor method for the Produce class for buildings
        /// </summary>
        public Produce(Sprite sprite, 
            int goldInc, int magicInc, int prodInc, int sciInc, int infInc, int festInc, int groInc,
            float goldMult, float magicMult, float prodMult, float sciMult, float infMult, float festMult, float groMult)
        {
            m_Sprite = sprite;
                m_goldInc = goldInc;
                m_magicInc = magicInc;
                m_prodInc = prodInc;
                m_sciInc = sciInc;
                m_infInc = infInc;
                m_festInc = festInc;
                m_groInc = groInc;

                m_goldMult = goldMult;
                m_magicMult = magicMult;
                m_prodMult = prodMult;
                m_sciMult = sciMult;
                m_infMult = infMult;
                m_festMult = festMult;
                m_groMult = groMult;

        }
    }
    /// <summary>
    /// A class named "hasYield" that inherits from "Unit"
    /// </summary>
    public class HasYield : Unit 
    {
        float gold; // Floating point variable named "gold"
        float magicka; // Floating point variable named "magicka"
        float science; // Floating point variable named "science"
        float influence; // Floating point variable named "influence"
        float festivity; // Floating point variable named "festivity"
        float production; // Floating point variable named "production"
        float growth; // Floating point variable named "growth"

        #region // A region for organizing related code
        /// <summary>
        /// A public method that returns the value of the "gold" variable
        /// </summary>
        /// <returns>the gold variable</returns>
        public float GetGold()
        {
            return gold;
        }

        public void AddGold(float f) // A public method that takes in a float parameter and adds it to the "gold" variable
        {
            gold += f;
        }
        public float GetMagicka() // A public method that returns the value of the "magicka" variable
        {
            return magicka;
        }

        public void AddMagicka(float f) // A public method that takes in a float parameter and adds it to the "magicka" variable
        {
            magicka += f;
        }

        public float GetGrowth() // A public method that returns the value of the "growth" variable
        {
            return growth;
        }

        public void AddGrowth(float f) // A public method that takes in a float parameter and adds it to the "growth" variable
        {
            growth += f;
        }

        public float GetProduction() // A public method that returns the value of the "production" variable
        {
            return production;
        }

        public void AddProduction(float f) // A public method that takes in a float parameter and adds it to the "production" variable
        {
            production += f;
        }

        public float GetScience() // A public method that returns the value of the "science" variable
        {
            return science;
        }

        public void AddScience(float f) // A public method that takes in a float parameter and adds it to the "science" variable
        {
            science += f;
        }

        public float GetInfluence() // A public method that returns the value of the "influence" variable
        {
            return influence;
        }

        public void AddInfluence(float f) // A public method that takes in a float parameter and adds it to the "influence" variable
        {
            influence += f;
        }

        public float GetFestivity() // A public method that returns the value of the "festivity" variable
        {
            return festivity;
        }

        public void AddFestivity(float f) // A public method that takes in a float parameter and adds it to the "festivity" variable
        {
            festivity += f;
        }

        #endregion // End of the region
    }
    /// <summary>
    /// A class representing a city in the game, with various properties such as position, name, and accumulated resources
    /// </summary>
    public class City : HasYield
    {
        Vector2 position; // The position of the city on the game map
        float growthAccum; // The accumulated growth of the city
        float influenceAccum; // The accumulated influence of the city
        float productionAccum; // The accumulated production of the city
        float festivityAccum; // The accumulated festivity of the city
        int cityLeader; // The index of the leader that owns the city
        int noOfCities; // The number of cities owned by the leader
        string name; // The name of the city
        List<Produce> cityProduce; // The list of produce that the city can produce
        List<Produce> purchasedProduce; // The list of produce that the city has purchased
        /// <summary>
        /// Constructor for the City class
        /// </summary>
        /// <param name="cityLeader_">The index of the leader that owns the city</param>
        /// <param name="noOfCities_">The number of cities owned by the leader</param>
        /// <param name="cityGameObject_">The GameObject representing the city</param>
        public City(int cityLeader_, int noOfCities_, GameObject cityGameObject_)
        {
            cityProduce = new List<Produce>();
            purchasedProduce = new List<Produce>();
            cityLeader = cityLeader_;
            noOfCities = noOfCities_;
            SetGameObject(cityGameObject_);
            Properties properties = new Properties();
            string[] words = properties.lines[cityLeader].Split(',');
            name = words[noOfCities];
        }
        /// <summary>
        /// Returns the name of the city
        /// </summary>
        /// <returns>The name of the city</returns>
        public string GetName()
        {
            return name;
        }
        /// <summary>
        /// Adds a Product to the list of produce that the city can produce
        /// </summary>
        /// <param name="product">The product to add</param>
        public void AddProduct(Produce product)
        {
            cityProduce.Add(product);
        }
        /// <summary>
        /// Adds a product to the list of produce that the city has purchased
        /// </summary>
        /// <param name="product">The Produce to add</param>
        public void PurchaseProduce(Produce product)
        {
            purchasedProduce.Add(product);
        }
        /// <summary>
        /// Adds an amount to the accumulated growth of the city
        /// </summary>
        /// <param name="f">The amount to add</param>
        public void AddGrowthAccum(float f)
        {
            growthAccum += f;
        }
        /// <summary>
        /// Adds an amount to the accumulated influence of the city
        /// </summary>
        /// <param name="f">The amount to add</param>
        public void AddInfluenceAccum(float f)
        {
            influenceAccum += f;
        }
        /// <summary>
        /// Adds an amount to the accumulated festivity of the city
        /// </summary>
        /// <param name="f">The amount to add</param>
        public void AddFestivityAccum(float f)
        {
            festivityAccum += f;
        }
        /// <summary>
        /// Adds an amount to the accumulated production of the city
        /// </summary>
        /// <param name="f">The amount to add</param>
        public void AddProductionAccum(float f)
        {
            festivityAccum += f;
        }
        /// <summary>
        /// Returns the list of produce that the city can produce
        /// </summary>
        /// <returns>The list of produce that the city can produce</returns>
        public override List<Produce> GetProduce()
        {
            return cityProduce;
        }
        /// <summary>
        /// Returns the accumulated growth of the city
        /// </summary>
        /// <returns>The accumulated growth of the city</returns>
        public float GetGrowthAccum()
        {
            return growthAccum;
        }
        /// <summary>
        /// Returns the accumulated influence of the city
        /// </summary>
        /// <returns>The accumulated influence of the city</returns>
        public float GetInfluenceAccum()
        {
            return influenceAccum;
        }
        /// <summary>
        /// Returns the accumulated production of the city
        /// </summary>
        /// <returns>The accumulated production of the city</returns>
        public float GetProductionAccum()
        {
            return productionAccum;
        }
        /// <summary>
        /// Returns the accumulated festivity of the city
        /// </summary>
        /// <returns>The accumulated festivity of the city</returns>
        public float GetFestivityAccum()
        {
            return festivityAccum;
        }
        /// <summary>
        /// Calculates and returns the total gold yield of the city, taking into account any purchased produce that affects gold yield
        /// </summary>
        /// <returns>The total gold yield of the city</returns>
        public float GetCityGold()
        {
            float totGold = GetGold();
            if (purchasedProduce.Count != 0)
            {
                foreach (Produce myProduct in purchasedProduce)
                {
                    totGold += myProduct.m_goldInc;
                }
                foreach (Produce myProduct in purchasedProduce)
                {
                    totGold *= myProduct.m_goldMult;
                }
            }
            return totGold;
        }
        /// <summary>
        /// Calculates and returns the total magicka yield of the city, taking into account any purchased produce that affects magicka yield
        /// </summary>
        /// <returns>The total magicka yield of the city</returns>
        public float GetCityMagicka()
        {
            float totMagicka = GetMagicka();
            if (purchasedProduce.Count != 0)
            {
                foreach (Produce myProduct in purchasedProduce)
                {
                    totMagicka += myProduct.m_magicInc;
                }
                foreach (Produce myProduct in purchasedProduce)
                {
                    totMagicka *= myProduct.m_magicMult;
                }
            }
            return totMagicka;
        }
        /// <summary>
        /// Calculates and returns the total production yield of the city, taking into account any purchased produce that affects production yield
        /// </summary>
        /// <returns>The total production yield of the city</returns>
        public float GetCityProduction()
        {
            float totProd = GetProduction();
            if (purchasedProduce.Count != 0)
            {
                foreach (Produce myProduct in purchasedProduce)
                {
                    totProd += myProduct.m_prodInc;
                }
                foreach (Produce myProduct in purchasedProduce)
                {
                    totProd *= myProduct.m_prodMult;
                }
            }
            return totProd;
        }
        /// <summary>
        /// Calculates and returns the total science yield of the city, taking into account any purchased produce that affects science yield
        /// </summary>
        /// <returns>The total science yield of the city</returns>
        public float GetCityScience()
        {
            float totSci = GetScience();
            if (purchasedProduce.Count != 0)
            {
                foreach (Produce myProduct in purchasedProduce)
                {
                    totSci += myProduct.m_sciInc;
                }
                foreach (Produce myProduct in purchasedProduce)
                {
                    totSci *= myProduct.m_sciMult;
                }
            }
            return totSci;
        }
        /// <summary>
        /// Calculates and returns the total influence yield of the city, taking into account any purchased produce that affects influence yield
        /// </summary>
        /// <returns>The total influence yield of the city</returns>
        public float GetCityInfluence()
        {
            float totInfluence = GetInfluence();
            if (purchasedProduce.Count != 0)
            {
                foreach (Produce myProduct in purchasedProduce)
                {
                    totInfluence += myProduct.m_infInc;
                }
                foreach (Produce myProduct in purchasedProduce)
                {
                    totInfluence *= myProduct.m_infMult;
                }
            }
            return totInfluence;
        }
        /// <summary>
        /// Calculates and returns the total festivity yield of the city, taking into account any purchased produce that affects festivity yield
        /// </summary>
        /// <returns>The total festivity yield of the city</returns>
        public float GetCityFestivity()
        {
            float totFest = GetFestivity();
            if (purchasedProduce.Count != 0)
            {
                foreach (Produce myProduct in purchasedProduce)
                {
                    totFest += myProduct.m_festInc;
                }
                foreach (Produce myProduct in purchasedProduce)
                {
                    totFest *= myProduct.m_festMult;
                }
            }
            return totFest;
        }
        /// <summary>
        /// Calculates and returns the total growth yield of the city, taking into account any purchased produce that affects growth yield
        /// </summary>
        /// <returns>The total growth yield of the city</returns>
        public float GetCityGrowth()
        {
            float totGro = GetGrowth();
            if (purchasedProduce.Count != 0)
            {
                foreach (Produce myProduct in purchasedProduce)
                {
                    totGro += myProduct.m_groInc;
                }
                foreach (Produce myProduct in purchasedProduce)
                {
                    totGro *= myProduct.m_groMult;
                }
            }
            return totGro;
        }
        /// <summary>
        /// The yield of the city based on the given index of the type of yield
        /// </summary>
        /// <param name="i">The index of the type of yield to get (0 for gold, 1 for magicka, 2 for production, 3 for science, 4 for influence, 5 for festivity and 6 for growth)</param>
        /// <returns>A string containing the yield value of the city based on the index provided</returns>
        public string GetYield(int i)
        {
            if (i == 0) // Returns the value of GetCityGold() if i equals 0
            {
                return GetCityGold().ToString();
            }
            if (i == 1) // Returns the value of GetCityMagicka() if i equals 1
            {
                return GetCityMagicka().ToString();
            }
            if (i == 2) // Returns the value of GetCityProduction() if i equals 2
            {
                return GetCityProduction().ToString();
            }
            if (i == 3) // Returns the value of GetCityScience() if i equals 3
            {
                return GetCityScience().ToString();
            }
            if (i == 4) // Returns the value of GetCityInfluence() if i equals 4
            {
                return GetCityInfluence().ToString();
            }
            if (i == 5) // Returns the value of GetCityFestivity() if i equals 5
            {
                return GetCityFestivity().ToString();
            }
            else // Otherwise, returns the value of GetCityGrowth()
            {
                return GetCityGrowth().ToString();
            }
        }
    }
    /// <summary>
    /// Class representing a game unit
    /// </summary>
    public class Unit
    {
        /// <summary>
        /// Enumeration of the types of game units
        /// </summary>
        public enum unitType { Passive, Military, Building };
        /// <summary>
        /// Enumeration of passive game unit types
        /// </summary>
        public enum passiveUnits { Settler, Labourer };
        /// <summary>
        /// Enumeration of military game unit types
        /// </summary>
        public enum militaryUnits { Scout, Axeman, Rock_Thrower };
        /// <summary>
        /// Enumeration of building game unit types
        /// </summary>
        public enum buildingUnits { City };
        int hexID; // ID of the hex where the unit is located
        int unitID; // ID of the unit
        private float unitHP; // Current hit points of the unit
        private float unitMaxHP; // Maximum hit points of the unit
        private float unitAttack; // Attack value of the unit
        private float unitDefense; // Defense value of the unit
        private float movement; // Remaining movement points of the unit
        private int maxMovement; // Maximum movement points of the unit
        private bool hasMoved = false; // Whether the unit has already moved in the current turn
        private bool isAquatic = false; // Whether the unit is aquatic and can move in water
        private Vector2 position; // Position of the unit in the game world
        private GameObject unitGameObject; // Unity game object representing the unit
        private GameObject billboardGameObject; // Unity game object representing the billboard of the unit
        private GameObject hpBarGameObject; // Unity game object representing the health bar of the unit
        private unitType unitUnitType; // Type of the unit
        private passiveUnits passiveUnitType; // Type of passive unit
        private militaryUnits militaryUnitType; // Type of military unit
        private buildingUnits buildingUnitType; // Type of building unit
        private HashSet<int> moveableTiles; // Set of IDs of hexes where the unit can move to
        /// <summary>
        /// Getter method for the unit's current hit points
        /// </summary>
        /// <returns>The current hit points of the unit</returns>
        public float GetHP()
        {
            return unitHP;
        }
        /// <summary>
        /// Getter method for the unit's maximum hit points
        /// </summary>
        /// <returns>The maximum hit points of the unit</returns>
        public virtual List<Produce> GetProduce()
        {
            throw new NotImplementedException();
        }
        public float GetMaxHP()
        {
            return unitMaxHP;
        }
        /// <summary>
        /// Getter method for the unit's attack value
        /// </summary>
        /// <returns>The attack value of the unit</returns>
        public float GetAttack()
        {
            return unitAttack;
        }
        /// <summary>
        /// Getter method for the unit's defense value
        /// </summary>
        /// <returns>The defense value of the unit</returns>
        public float GetDefense()
        {
            return unitDefense;
        }
        /// <summary>
        /// Getter method for the unit's passive unit type
        /// </summary>
        /// <returns>The type of passive unit</returns>
        public passiveUnits GetPassiveUnitType()
        {
            return passiveUnitType;
        }
        /// <summary>
        /// Getter method for the unit's military unit type
        /// </summary>
        /// <returns>The type of military unit</returns>
        public militaryUnits GetMilitaryUnitType()
        {
            return militaryUnitType;
        }
        /// <summary>
        /// Getter method for the unit's building unit type
        /// </summary>
        /// <returns>The type of building unit</returns>
        public buildingUnits GetBuildingUnitType()
        {
            return buildingUnitType;
        }
        /// <summary>
        /// Getter method for the unit's type
        /// </summary>
        /// <returns>The type of unit</returns>
        public unitType GetUnitType()
        {
            return unitUnitType;
        }
        /// <summary>
        /// Whether the unit has moved during the current turn
        /// </summary>
        /// <returns>A boolean value indicating whether the unit has moved or not</returns>
        public bool GetMoved()
        {
            return hasMoved;
        }
        /// <summary>
        /// The hex ID of the hex the unit is currently occupying
        /// </summary>
        /// <returns>An integer value representing the hex ID</returns>
        public int GetHexID()
        {
            return hexID;
        }
        /// <summary>
        /// The ID of the unit
        /// </summary>
        /// <returns>An integer value representing the unit ID</returns>
        public int GetID()
        {
            return unitID;
        }
        /// <summary>
        /// Whether the unit is aquatic or not
        /// </summary>
        /// <returns>A boolean value indicating whether the unit is aquatic or not</returns>
        public bool GetAquatic()
        {
            return isAquatic;
        }
        /// <summary>
        /// The current position of the unit
        /// </summary>
        /// <returns>A Vector2 representing the position of the unit</returns>
        public Vector2 GetPosition()
        {
            return position;
        }
        /// <summary>
        /// The game object representing the unit
        /// </summary>
        /// <returns>A GameObject representing the unit</returns>
        public GameObject GetGameObject()
        {
            return unitGameObject;
        }
        /// <summary>
        /// The game object representing the billboard of the unit
        /// </summary>
        /// <returns>A GameObject representing the billboard of the unit</returns>
        public GameObject GetBillboardGameObject()
        {
            return billboardGameObject;
        }
        /// <summary>
        /// The game object representing the health bar of the unit
        /// </summary>
        /// <returns>A GameObject representing the health bar of the unit</returns>
        public GameObject GetHPBarGameObject()
        {
            return hpBarGameObject;
        }
        /// <summary>
        /// The current movement value of the unit
        /// </summary>
        /// <returns>A float representing the current movement value of the unit</returns>
        public float GetMovement()
        {
            return movement;
        }
        /// <summary>
        /// The maximum movement value of the unit
        /// </summary>
        /// <returns>An integer representing the maximum movement value of the unit</returns>
        public int GetMaxMovement()
        {
            return maxMovement;
        }
        /// <summary>
        /// A HashSet of hex IDs representing the tiles the unit can move to
        /// </summary>
        /// <returns>A HashSet of integers representing the moveable tiles</returns>
        public HashSet<int> GetMoveableTiles()
        {
            return moveableTiles;
        }
        /// <summary>
        /// Sets the defense value of the unit
        /// </summary>
        /// <param name="_unitDefense">A float representing the defense value to set</param>
        public void SetDefense(float _unitDefense)
        {
            unitDefense = _unitDefense;
        }
        /// <summary>
        /// The attack value of the unit
        /// </summary>
        /// <param name="_unitAttack">A float representing the attack value to set</param>
        public void SetAttack(float _unitAttack)
        {
            unitAttack = _unitAttack;
        }
        /// <summary>
        /// Sets the current health value of the unit
        /// </summary>
        /// <param name="_unitHP">A float representing the current health value to set</param>
        public void SetHP(float _unitHP)
        {
            unitHP = _unitHP;
        }
        /// <summary>
        /// Sets the maximum health value of the unit
        /// </summary>
        /// <param name="_unitMaxHP">A float representing the maximum health value to set</param>
        public void SetMaxHP(float _unitMaxHP)
        {
            unitMaxHP = _unitMaxHP;
        }
        /// <summary>
        /// Sets the hex ID of the hex the unit is currently occupying
        /// </summary>
        /// <param name="_hexID">An integer value representing the hex ID to set</param>
        public void SetHexID(int _hexID)
        {
            hexID = _hexID;
        }
        /// <summary>
        /// Sets the ID of the unit
        /// </summary>
        /// <param name="_unitID">An integer value representing the unit ID to set</param>
        public void SetUnitID(int _unitID)
        {
            unitID = _unitID;
        }
        /// <summary>
        /// Sets whether the unit is aquatic or not
        /// </summary>
        /// <param name="_isAquatic">A boolean value representing if the unit is aquatic or not</param>
        public void SetAquatic(bool _isAquatic)
        {
            isAquatic = _isAquatic;
        }
        /// <summary>
        /// Sets whether the position of the unit
        /// </summary>
        /// <param name="_position">The position of the unit</param>
        public void SetPosition(Vector2 _position)
        {
            position = _position;
        }
        /// <summary>
        /// Sets the indicator for if the unit has moved or not
        /// </summary>
        public void SetMoved()
        {
            hasMoved = true;
        }
        /// <summary>
        /// Sets the Game Object for the unit
        /// </summary>
        /// <param name="_unitGameObject">The unit GameObject</param>
        public void SetGameObject(GameObject _unitGameObject)
        {
            unitGameObject = _unitGameObject;
        }
        /// <summary>
        /// Sets the remaining movement points of the unit
        /// </summary>
        /// <param name="_movement">The remaining movement points for the unit</param>
        public void SetMovement(float _movement)
        {
            movement = _movement;
        }
        /// <summary>
        /// Sets the maximum movement value for the unit
        /// </summary>
        /// <param name="_maxMovement">The maximum movement value for the unit</param>
        public void SetMaxMovement(int _maxMovement)
        {
            maxMovement = _maxMovement;
        }
        /// <summary>
        /// Sets the billboard GameObject for the unit
        /// </summary>
        /// <param name="_billboardGameObject">The billboard GameObject</param>
        public void SetBillboardGameObject(GameObject _billboardGameObject)
        {
            billboardGameObject = _billboardGameObject;
        }
        ///
        /// <summary>
        /// Sets the billboard GameObject for the unit
        /// </summary>
        /// <param name="_hpBarGameObject"></param>
        public void SetHPBarGameObject(GameObject _hpBarGameObject)
        {
            hpBarGameObject = _hpBarGameObject;
        }
        /// <summary>
        /// Sets the unit type the passive unit
        /// </summary>
        /// <param name="typeOfUnit">The type of passive unit</param>
        public void SetPassiveUnit(passiveUnits typeOfUnit)
        {
            unitUnitType = unitType.Passive;
            passiveUnitType = typeOfUnit;
        }
        /// <summary>
        /// Sets the unit type the military unit
        /// </summary>
        /// <param name="typeOfUnit">The type of military unit</param>
        public void SetMilitaryUnit(militaryUnits typeOfUnit)
        {
            unitUnitType = unitType.Military;
            militaryUnitType = typeOfUnit;
        }
        /// <summary>
        /// Sets the unit type the building unit
        /// </summary>
        /// <param name="typeOfUnit">The type of building unit</param>
        public void SetBuildingUnit(buildingUnits typeOfUnit)
        {
            unitUnitType = unitType.Building;
            buildingUnitType = typeOfUnit;
        }
        /// <summary>
        /// Create a hashmap of indexes of tiles which the unit can move to
        /// </summary>
        /// <param name="_moveableTiles">Hashmap of indexes of tiles which the unit can move to</param>
        public void SetMoveableTiles(HashSet<int> _moveableTiles)
        {
            moveableTiles = _moveableTiles;
        }
    }
    /// <summary>
    /// Military Unit unit type
    /// </summary>
    public class MilitaryUnit : Unit
    {

    }
    /// <summary>
    /// Passive Unit unit type
    /// </summary>
    public class PassiveUnit : Unit
    {

    }
    /// <summary>
    /// A class that represents a player in the game
    /// Inherits from HasYield
    /// </summary>
    int leader; // The leader's ID.
    bool isHuman = false; // Indicates whether the player is human or not.
    Vector3 startingPosition; // The starting position of the player.
    List<City> cities = new List<City>(); // A list of cities owned by the player.
    List<PassiveUnit> m_passiveUnits = new List<PassiveUnit>(); // A list of passive units owned by the player.
    List<MilitaryUnit> m_militaryUnits = new List<MilitaryUnit>(); // A list of military units owned by the player.
    Unit currentUnit; // The current unit selected by the player.
    public class Player : HasYield
    {
        int leader; // The leader's ID.
        bool isHuman = false; // Indicates whether the player is human or not.
        Vector3 startingPosition; // The starting position of the player.
        List<City> cities = new List<City>(); // A list of cities owned by the player.
        List<PassiveUnit> m_passiveUnits = new List<PassiveUnit>(); // A list of passive units owned by the player.
        List<MilitaryUnit> m_militaryUnits = new List<MilitaryUnit>(); // A list of military units owned by the player.
        Unit currentUnit; // The current unit selected by the player.
        /// <summary>
        /// Checks if the player owns a given unit
        /// </summary>
        /// <param name="unitToCheck">unitToCheck The unit to check if the player owns</param>
        /// <returns>return true if the player owns the unit, false otherwise</returns>
        public bool Owns(Unit unitToCheck)
        {
            if (unitToCheck.GetUnitType() == unitType.Passive)
            {
                foreach (PassiveUnit pass in m_passiveUnits)
                {
                    if (pass.GetID() == unitToCheck.GetID())
                    {
                        return true;
                    }
                }
            }

            if (unitToCheck.GetUnitType() == unitType.Building)
            {
                foreach (City city in cities)
                {
                    if (city.GetHexID() == unitToCheck.GetHexID())
                    {
                        return true;
                    }
                }
            }

            if (unitToCheck.GetUnitType() == unitType.Military)
            {
                foreach (MilitaryUnit mil in m_militaryUnits)
                {
                    if (mil.GetHexID() == unitToCheck.GetHexID())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Removes a unit from the player's unit list
        /// </summary>
        /// <param name="rUnit">Unit The unit to remove</param>
        public void RemoveUnit(Unit rUnit)
        {
            for (int i = 0; i < m_passiveUnits.Count; i++)
            {
                if (m_passiveUnits[i].GetID() == rUnit.GetID())
                {
                    m_passiveUnits.RemoveAt(i);
                };
            }
            for (int j = 0; j < cities.Count; j++)
            {
                if (cities[j].GetID() == rUnit.GetID())
                {
                    cities.RemoveAt(j);
                };
            }
            for (int k = 0; k < m_militaryUnits.Count; k++)
            {
                if (m_militaryUnits[k].GetID() == rUnit.GetID())
                {
                    m_militaryUnits.RemoveAt(k);
                };
            }
        }
        /// <summary>
        /// Get a new unit from the player's list of units.
        /// If there are military units, return the first one found.
        /// If there are no military units, check for cities and return the first one found.
        /// If there are no cities, check for passive units and return the first one found.
        /// If there are no units, return null.
        /// </summary>
        /// <returns>The new unit to be added to the player's list of units</returns>
        public Unit GetNewUnit()
        {
            foreach (MilitaryUnit mil in m_militaryUnits)
            {
                return mil;
            }
            foreach (City city in cities)
            {
                return city;
            }
            foreach (PassiveUnit pass in m_passiveUnits)
            {
                return pass;
            }
            return null;
        }
        /// <summary>
        /// Get a list of passive units belonging to the player
        /// </summary>
        /// <returns>The list of passive units belonging to the player</returns>
        public List<PassiveUnit> GetPassiveUnits()
        {
            return m_passiveUnits;
        }
        /// <summary>
        /// Get a list of military units belonging to the player
        /// </summary>
        /// <returns>The list of military units belonging to the player</returns>
        public List<MilitaryUnit> GetMilitaryUnits()
        {
            return m_militaryUnits;
        }
        /// <summary>
        /// Get a list of all units belonging to the player
        /// </summary>
        /// <returns>The list of all units belonging to the player</returns>
        public List<Unit> GetUnits()
        {
            List<Unit> returnUnits = new List<Unit>();
            foreach (City city in cities)
            {
                returnUnits.Add(city);
            }
            foreach (PassiveUnit pu in m_passiveUnits)
            {
                returnUnits.Add(pu);
            }
            foreach (MilitaryUnit mu in m_militaryUnits)
            {
                returnUnits.Add(mu);
            }
            return returnUnits;
        }
        /// <summary>
        /// Set the player as human-controlled
        /// </summary>
        public void SetHuman()
        {
            isHuman = true;
        }
        /// <summary>
        /// Set the player's leader index
        /// </summary>
        /// <param name="_leader">The leader index to be set<param>
        public void SetLeader(int _leader)
        {
            leader = _leader;
        }
        /// <summary>
        /// Get the player's leader index
        /// </summary>
        /// <returns>The player's leader index</returns>
        public int GetLeader()
        {
            return leader;
        }
        /// <summary>
        /// Get whether the player is human-controlled
        /// </summary>
        /// <returns>Whether the player is human-controlled</returns>
        public bool GetHuman()
        {
            return isHuman;
        }
        /// <summary>
        /// Set the starting position for the player
        /// </summary>
        /// <param name="_startingPosition">The starting position to be set</param>
        public void SetStartingPosition(Vector3 _startingPosition)
        {
            startingPosition = _startingPosition;
        }
        /// <summary>
        /// Set the current unit that the player is controlling
        /// </summary>
        /// <param name="unit">The current unit to be set</param>
        public void SetCurrentUnit(Unit unit)
        {
            currentUnit = unit;
        }
        /// <summary>
        /// Get the current unit that the player is controlling
        /// </summary>
        /// <returns>The current unit that the player is controlling</returns>
        public Unit GetCurrentUnit()
        {
            return currentUnit;
        }
        /// <summary>
        /// Get the starting position for the player
        /// </summary>
        /// <returns>The starting position for the player</returns>
        public Vector3 GetStartingPosition()
        {
            return startingPosition;
        }
        /// <summary>
        /// Settle a new city for the player
        /// Add starting resources and add the new city to the player's list of cities
        /// </summary>
        /// <param name="newCity">The new city to be settled</param>
        public void SettleCity(City newCity)
        {
            newCity.AddFestivity(1);
            newCity.AddGold(1);
            newCity.AddGrowth(1);
            newCity.AddInfluence(1);
            newCity.AddMagicka(1);
            newCity.AddProduction(1);
            newCity.AddScience(1);
            cities.Add(newCity);
        }
        /// <summary>
        /// Remove the first city from the list of cities
        /// </summary>
        public void RemoveCity()
        {
            cities.RemoveAt(0);
        }
        /// <summary>
        /// Get a city at a given index from the list of cities
        /// </summary>
        /// <param name="i">The index of the city to get</param>
        /// <returns>The city at the specified index</returns>
        public City GetCity(int i)
        {
            return cities[i];
        }
        /// <summary>
        /// Get a list of all cities
        /// </summary>
        /// <returns>A list of all cities</returns>
        public List<City> GetCities()
        {
            return cities;
        }
        /// <summary>
        /// Update the resources of the player based on the resources of their cities
        /// </summary>
        public void GetIncome()
        {
            foreach(City city in cities)
            {
                AddGold(city.GetCityGold());
                AddMagicka(city.GetCityMagicka());
                AddProduction(city.GetCityProduction());
                AddScience(city.GetCityScience());
                AddInfluence(city.GetCityInfluence());
                AddFestivity(city.GetCityFestivity());
                AddGrowth(city.GetCityGrowth());

                city.AddProductionAccum(city.GetCityProduction());
                city.AddInfluenceAccum(city.GetCityInfluence());
                city.AddFestivityAccum(city.GetCityFestivity());
                city.AddGrowthAccum(city.GetCityGrowth());
            }
        }
        /// <summary>
        /// Add a passive unit to the list of passive units
        /// </summary>
        /// <param name="passiveUnit">The passive unit to add</param>
        public void AddPassiveUnit(PassiveUnit passiveUnit)
        {
            m_passiveUnits.Add(passiveUnit);
        }
        /// <summary>
        /// Add a military unit to the list of military units
        /// </summary>
        /// <param name="militaryUnit">The military unit to add</param>
        public void AddMilitaryUnit(MilitaryUnit militaryUnit)
        {
            m_militaryUnits.Add(militaryUnit);
        }
    }
}
