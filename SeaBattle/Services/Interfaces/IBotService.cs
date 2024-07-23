using Microsoft.AspNetCore.Identity.Data;
using SeaBattle.Consts;
using SeaBattle.Models;
using SeaBattle.Models.AuxilaryModels;
using System.Reflection.Metadata;

namespace SeaBattle.Services.Interfaces;

public interface IBotService
{
    public bool Shoot(Table table)
    {
        Random random = new Random();
        int x = random.Next(0, Constants.TableWidth);
        int y = random.Next(0, Constants.TableWidth);
        bool result;// =   //Shoot(table, x, y);
        


        return true;
    }
}
