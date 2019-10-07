using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GADETask1
{
    [Serializable]
    public class GameEngine
    {
        
        public Map map;
        private int round;
        int resources;
        Random r = new Random();
        int UnitCost = 10;
        GroupBox MapBox;

        public int Round
        {
            get { return round; }
        }
   

        public GameEngine(int numBuildings, GroupBox gMap,int mapX,int mapY)
        {
            MapBox = gMap;
            map = new Map(numBuildings,mapX,mapY);
            map.Generate();
           

            round = 1; 
        }

        public void Update()
        {
            foreach (Building b in map.Buildings)
            {
                if (b is FactoryBuilding)
                {
                    FactoryBuilding f = (FactoryBuilding)b;
                    if (f.Production_Speed % round == 0 && resources >= UnitCost) // this allows for units to spawn only if there are enough resources
                    {
                        map.Units.Add(f.SpawnUnit());
                    }
                }
                if (b is ResourceBuilding)
                {
                    ResourceBuilding rs = (ResourceBuilding)b;
                    resources=rs.GenerateResources();// the generate resources method is called every turn in the update method
                }
            }
            for (int i = 0; i < map.Units.Count; i++)
            {
                if (map.Units[i] is MeleeUnit)
                {
                    MeleeUnit mu = (MeleeUnit)map.Units[i];
                    if (mu.health <= mu.maxHealth * 0.25) // Running Away
                    {
                        mu.Move(r.Next(0, 4));
                    }
                    else
                    {
                        (Unit closest, int distanceTo) = mu.EnemyDistance(map.Units);
                        (Building closestbuilding, int distance) = mu.ClosestBuilding(map.Buildings);
                        if (distanceTo < distance)  //Checks if Unit is closer than building
                        {
                            //Check In Range
                            if (distanceTo <= mu.attackRange)
                            {
                                mu.isAttack = true;
                                mu.Combat(closest);
                                if (mu.IsDead == true)   //Returns resources if unit is dead
                                {
                                    foreach (Building bg in map.Buildings)
                                    {
                                        if (bg is ResourceBuilding)
                                        {
                                            ResourceBuilding rb = (ResourceBuilding)bg;
                                            if (mu.team == rb.team)
                                            {
                                                rb.ResourcesGenerated++;
                                            }
                                        }
                                    }
                                }
                            }
                            else //Move Towards
                            {
                                if (closest is MeleeUnit)
                                {
                                    MeleeUnit closestMu = (MeleeUnit)closest;
                                    if (mu.xPos > closestMu.xPos) //North
                                    {
                                        mu.Move(0);
                                    }
                                    else if (mu.xPos < closestMu.xPos) //South
                                    {
                                        mu.Move(2);
                                    }
                                    else if (mu.yPos > closestMu.yPos) //West
                                    {
                                        mu.Move(3);
                                    }
                                    else if (mu.yPos < closestMu.yPos) //East
                                    {
                                        mu.Move(1);
                                    }
                                }
                                else if (closest is RangedUnit)
                                {
                                    RangedUnit closestRu = (RangedUnit)closest;
                                    if (mu.xPos > closestRu.xPos) //North
                                    {
                                        mu.Move(0);
                                    }
                                    else if (mu.xPos < closestRu.xPos) //South
                                    {
                                        mu.Move(2);
                                    }
                                    else if (mu.yPos > closestRu.yPos) //West
                                    {
                                        mu.Move(3);
                                    }
                                    else if (mu.yPos < closestRu.yPos) //East
                                    {
                                        mu.Move(1);
                                    }
                                }
                                if (closest is WizzardUnit)
                                {
                                    WizzardUnit closestwu = (WizzardUnit)closest;
                                    if (mu.xPos > closestwu.xPos) //North
                                    {
                                        mu.Move(0);
                                    }
                                    else if (mu.xPos < closestwu.xPos) //South
                                    {
                                        mu.Move(2);
                                    }
                                    else if (mu.yPos > closestwu.yPos) //West
                                    {
                                        mu.Move(3);
                                    }
                                    else if (mu.yPos < closestwu.yPos) //East
                                    {
                                        mu.Move(1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Check In Range
                            if (distanceTo <= mu.attackRange)
                            {
                                mu.isAttack = true;
                                mu.Combat(closest);
                            }
                            else //Move Towards
                            {
                                if (closestbuilding is FactoryBuilding)//units can move towwards buildings too
                                {
                                    FactoryBuilding closestfb = (FactoryBuilding)closestbuilding;
                                    if (mu.xPos > closestfb.xPos) //North
                                    {
                                        mu.Move(1);
                                    }
                                    else if (mu.xPos < closestfb.xPos) //South
                                    {
                                        mu.Move(2);
                                    }
                                    else if (mu.yPos > closestfb.yPos) //West
                                    {
                                        mu.Move(3);
                                    }
                                    else if (mu.yPos < closestfb.yPos) //East
                                    {
                                        mu.Move(4);
                                    }
                                }
                                else if (closestbuilding is ResourceBuilding)
                                {
                                    ResourceBuilding closestrb = (ResourceBuilding)closestbuilding;
                                    if (mu.xPos > closestrb.xPos) //North
                                    {
                                        mu.Move(1);
                                    }
                                    else if (mu.xPos < closestrb.xPos) //South
                                    {
                                        mu.Move(2);
                                    }
                                    else if (mu.yPos > closestrb.yPos) //West
                                    {
                                        mu.Move(3);
                                    }
                                    else if (mu.yPos < closestrb.yPos) //East
                                    {
                                        mu.Move(4);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (map.Units[i] is RangedUnit)
                {
                    RangedUnit ru = (RangedUnit)map.Units[i];
                    if (ru.health <= ru.maxHealth * 0.25) 
                    {
                        ru.Move(r.Next(0, 4));
                    }
                    else
                    {
                        (Unit closest, int distanceTo) = ru.EnemyDistance(map.Units);
                        (Building closestbuilding, int distance) = ru.ClosestBuilding(map.Buildings);
                        if (distanceTo < distance)  //Checks if Unit is closer than building
                        {
                            //Check In Range
                            if (distanceTo <= ru.attackRange)
                        {
                            ru.isAttack = true;
                            ru.Combat(closest);
                                if (ru.IsDead == true)   //Returns resources if unit is dead
                                {
                                    foreach (Building bg in map.Buildings)
                                    {
                                        if (bg is ResourceBuilding)
                                        {
                                            ResourceBuilding rb = (ResourceBuilding)bg;
                                            if (ru.team == rb.team)
                                            {
                                                rb.ResourcesGenerated++;
                                            }
                                        }
                                    }
                                }
                            }
                        else //Move Towards
                        {
                            if (closest is MeleeUnit)
                            {
                                MeleeUnit closestMu = (MeleeUnit)closest;
                                if (ru.xPos > closestMu.xPos) //North
                                {
                                    ru.Move(0);
                                }
                                else if (ru.xPos < closestMu.xPos) //South
                                {
                                    ru.Move(2);
                                }
                                else if (ru.yPos > closestMu.yPos) //West
                                {
                                    ru.Move(3);
                                }
                                else if (ru.yPos < closestMu.yPos) //East
                                {
                                    ru.Move(1);
                                }
                            }
                            else if (closest is RangedUnit)
                            {
                                RangedUnit closestRu = (RangedUnit)closest;
                                if (ru.xPos > closestRu.xPos) //North
                                {
                                    ru.Move(0);
                                }
                                else if (ru.xPos < closestRu.xPos) //South
                                {
                                    ru.Move(2);
                                }
                                else if (ru.yPos > closestRu.yPos) //West
                                {
                                    ru.Move(3);
                                }
                                else if (ru.yPos < closestRu.yPos) //East
                                {
                                    ru.Move(1);
                                }
                            }
                                if (closest is WizzardUnit)
                                {
                                    WizzardUnit closestMu = (WizzardUnit)closest;
                                    if (ru.xPos > closestMu.xPos) //North
                                    {
                                        ru.Move(0);
                                    }
                                    else if (ru.xPos < closestMu.xPos) //South
                                    {
                                        ru.Move(2);
                                    }
                                    else if (ru.yPos > closestMu.yPos) //West
                                    {
                                        ru.Move(3);
                                    }
                                    else if (ru.yPos < closestMu.yPos) //East
                                    {
                                        ru.Move(1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Check In Range
                            if (distanceTo <= ru.attackRange)
                            {
                                ru.isAttack = true;
                                ru.Combat(closest);
                            }
                            else //Move Towards
                            {
                                if (closestbuilding is FactoryBuilding)
                                {
                                    FactoryBuilding closestfb = (FactoryBuilding)closestbuilding;
                                    if (ru.xPos > closestfb.xPos) //North
                                    {
                                        ru.Move(1);
                                    }
                                    else if (ru.xPos < closestfb.xPos) //South
                                    {
                                        ru.Move(2);
                                    }
                                    else if (ru.yPos > closestfb.yPos) //West
                                    {
                                        ru.Move(3);
                                    }
                                    else if (ru.yPos < closestfb.yPos) //East
                                    {
                                        ru.Move(4);
                                    }
                                }
                                else if (closestbuilding is ResourceBuilding)
                                {
                                    ResourceBuilding closestrb = (ResourceBuilding)closestbuilding;
                                    if (ru.xPos > closestrb.xPos) //North
                                    {
                                        ru.Move(1);
                                    }
                                    else if (ru.xPos < closestrb.xPos) //South
                                    {
                                        ru.Move(2);
                                    }
                                    else if (ru.yPos > closestrb.yPos) //West
                                    {
                                        ru.Move(3);
                                    }
                                    else if (ru.yPos < closestrb.yPos) //East
                                    {
                                        ru.Move(4);
                                    }
                                }
                            }
                        }

                    }

                }
                else if (map.Units[i] is WizzardUnit)
                {
                    WizzardUnit wu = (WizzardUnit)map.Units[i];
                    if (wu.health <= wu.maxHealth * 0.50) // Wizzards will run away when on half health or less
                    {
                        wu.Move(r.Next(0, 4));
                    }
                    else
                    {
                        (Unit closest, int distanceTo) = wu.EnemyDistance(map.Units);

                        //Check In Range
                        if (distanceTo <= wu.attackRange)
                        {
                            wu.isAttack = true;
                            wu.Combat(closest);
                        }
                        else //Move Towards
                        {
                            if (closest is MeleeUnit)
                            {
                                MeleeUnit closestMu = (MeleeUnit)closest;
                                if (wu.xPos > closestMu.xPos) //North
                                {
                                    wu.Move(0);
                                }
                                else if (wu.xPos < closestMu.xPos) //South
                                {
                                    wu.Move(2);
                                }
                                else if (wu.yPos > closestMu.yPos) //West
                                {
                                    wu.Move(3);
                                }
                                else if (wu.yPos < closestMu.yPos) //East
                                {
                                    wu.Move(1);
                                }
                            }
                            else if (closest is RangedUnit)
                            {
                                RangedUnit closestRu = (RangedUnit)closest;
                                if (wu.xPos > closestRu.xPos) //North
                                {
                                    wu.Move(0);
                                }
                                else if (wu.xPos < closestRu.xPos) //South
                                {
                                    wu.Move(2);
                                }
                                else if (wu.yPos > closestRu.yPos) //West
                                {
                                    wu.Move(3);
                                }
                                else if (wu.yPos < closestRu.yPos) //East
                                {
                                    wu.Move(1);
                                }
                            }
                            
                        }

                    }

                }
            }
            
            round++;
            
        }

        public int DistanceTo(Unit a, Unit b)
        {
            int distance = 0;

            if (a is MeleeUnit && b is MeleeUnit)
            {
                MeleeUnit start = (MeleeUnit)a;
                MeleeUnit end = (MeleeUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is RangedUnit && b is MeleeUnit)
            {
                RangedUnit start = (RangedUnit)a;
                MeleeUnit end = (MeleeUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is RangedUnit && b is RangedUnit)
            {
                RangedUnit start = (RangedUnit)a;
                RangedUnit end = (RangedUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is MeleeUnit && b is RangedUnit)
            {
                MeleeUnit start = (MeleeUnit)a;
                RangedUnit end = (RangedUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is MeleeUnit && b is WizzardUnit)
            {
                MeleeUnit start = (MeleeUnit)a;
                WizzardUnit end = (WizzardUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is RangedUnit && b is WizzardUnit)
            {
                RangedUnit start = (RangedUnit)a;
                WizzardUnit end = (WizzardUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is WizzardUnit && b is WizzardUnit)
            {
                WizzardUnit start = (WizzardUnit)a;
                WizzardUnit end = (WizzardUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is WizzardUnit && b is MeleeUnit)
            {
                WizzardUnit start = (WizzardUnit)a;
                MeleeUnit end = (MeleeUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            else if (a is WizzardUnit && b is RangedUnit)
            {
                WizzardUnit start = (WizzardUnit)a;
                RangedUnit end = (RangedUnit)b;
                distance = Math.Abs(start.xPos - end.xPos) + Math.Abs(start.yPos - end.yPos);
            }
            return distance;
        }
        public void Save()// the save method saves the position of everything on the map and saves it to a text file
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream("Map.bat", FileMode.Create, FileAccess.Write, FileShare.None);

            using (fileStream)
            {
                binaryFormatter.Serialize(fileStream, map);

                MessageBox.Show("Saved");
            }
        }

        public void Load()// the load function reads a text file and places everything on the map
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream("Map.bat", FileMode.Open, FileAccess.Read, FileShare.None);

            using (fileStream)
            {
                map = (Map)formatter.Deserialize(fileStream);

                MessageBox.Show("Game Loaded");
            }
        }

    }
}
