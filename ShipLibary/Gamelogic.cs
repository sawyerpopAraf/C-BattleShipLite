using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipLibary.Models;

namespace ShipLibary
{
    public static class GameLogic
    {
        public static void IntializeGrid(PlayerInfoModel model){
            List<string> letters=[
                "A","B","C","D","E"
            ];
            List<int> numbers=[1,2,3,4,5];

            foreach(string letter in letters){
                foreach(int number in numbers){
                   AddGridSpot(model,letter,number);
                }
            }
            }

           private static void AddGridSpot(PlayerInfoModel model,string letter, int number){
            GridSpotModel spot = new()
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };
            model.ShotGrid.Add(spot);
        }

        public static bool PlaceShip(PlayerInfoModel model, string location) {
             (string row , int column)=SplitShotIntoRowAndColumn(location); 
             bool output=false;
             
             bool isValidLocation=ValidateGridLocation(model,row,column);
             bool isSpotOpen=ValidateShipLocation(model,row,column);
            
             if(isValidLocation&& isSpotOpen){
                   model.ShipLocations.Add(new GridSpotModel{
                    SpotLetter=row.ToUpper(),
                    SpotNumber=column,
                    Status=GridSpotStatus.Ship
                   });
                   output=true;
             }
             return output;
        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation=true;
            foreach(var ship in model.ShipLocations){
                if(ship.SpotLetter==row.ToUpper() && ship.SpotNumber==column){
                    isValidLocation=false;
                };
            }
            return isValidLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidLocation=false;
            foreach(var ship in model.ShotGrid){
                if(ship.SpotLetter==row.ToUpper() && ship.SpotNumber==column){
                    isValidLocation=true;
                };
            }
            return isValidLocation;
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActivate=false;
            foreach(var ship in player.ShipLocations){
                if(ship.Status!=GridSpotStatus.Sunk){
                    isActivate=true;
                }
            }
            return isActivate;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount=0;
            foreach(var shot in player.ShotGrid){
                if(shot.Status!=GridSpotStatus.Empty){
                    shotCount++;
                }
            }
            return shotCount;
        }

        public static (string row, int column) SplitShotIntoRowAndColumn(string shot)
        {
             if(shot.Length!=2){
                throw new ArgumentException("This was an invalid shot input",nameof(shot)); 
             }
            char[] shotArray=shot.ToArray();
            
            string row=shotArray[0].ToString().ToUpper();
            
            int column=int.Parse(shotArray[1].ToString());

            return (row,column);
        }

        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValidShot=false;
            
            foreach(var gridSpot in player.ShotGrid){
               if(gridSpot.SpotLetter==row.ToUpper()&& gridSpot.SpotNumber==column){
                if(gridSpot.Status==GridSpotStatus.Empty){
                    isValidShot=true;
                }
               }
            }
            return isValidShot;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
             bool isAHit=false;
            
            foreach(var ship in opponent.ShipLocations){
                if(ship.SpotLetter==row.ToUpper() && ship.SpotNumber==column){
                    
                    isAHit=true;
                    ship.Status=GridSpotStatus.Sunk;
                };
            }
            return isAHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
        {
            foreach(var gridSpot in player.ShotGrid){
                
                if(gridSpot.SpotLetter==row.ToUpper() && gridSpot.SpotNumber==column){
                    if(isAHit){
                        gridSpot.Status=GridSpotStatus.Hit;
                    }
                    else{
                        gridSpot.Status=GridSpotStatus.Miss;
                    }
                };
            }
           
        }
    }

}