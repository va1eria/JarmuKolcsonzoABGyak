﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JarmuKolcsonzoABGyak
{
    static class ABKezelo
    {
        static SqlConnection connection;
        static SqlCommand command;

        static void Csatlakozas()
        {
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["KolcsonzokDBConStr"].ConnectionString;
                connection.Open();
                command = new SqlCommand();
                command.Connection = connection;
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen csatlakozas!", ex);
            }
        }

        static void KapcsolatBontas()
        {
            try
            {
                connection.Close();
                command.Dispose();
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen kapcsolatbontas!", ex);
            }
        }

        // CRUD muvletek

        static void TelepulesSzinkronizalasa(Cim beszur)
        {
            command.CommandText = "SELECT [IRSZ] FROM [Telepules] WHERE [IRSZ] = @irsz";
            command.Parameters.AddWithValue("@irsz", beszur.Irsz.ToString());
            bool voltIlyen = false;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                voltIlyen = reader.HasRows;
            }
            if (!voltIlyen)
            {
                command.CommandText = "INSERT INTO [Telepules] VALUES (@irsz, @telep)";
                command.Parameters.AddWithValue("@telep", beszur.Telepules);
                command.ExecuteNonQuery();
            }
        }
        public static void KolcsonzoFelvitel(Kolcsonzo uj)
        {
            Csatlakozas();

            try
            {
                // check hogy letezik-e a telepules, ha nem akkor letre kell hozni
                command.Transaction = connection.BeginTransaction();
                TelepulesSzinkronizalasa(uj.Cim);
                // kolcsonzo beszurasa
                command.Parameters.Clear();
                command.CommandText = "INSERT INTO [Kolcsonzo]([IRSZ], [Kozterulet], [KozteruletJellege], [Hazszam], [Megnevezes]) OUTPUT INSERTED.Id VALUES (@irsz, @kozt, @koztj, @hsz, @megn)";
                command.Parameters.AddWithValue("@irsz", uj.Cim.Irsz.ToString());
                command.Parameters.AddWithValue("@kozt", uj.Cim.Kozterulet);
                command.Parameters.AddWithValue("@koztj", (int)uj.Cim.KozteruletJellege);
                command.Parameters.AddWithValue("@hsz", uj.Cim.Hazszam);
                command.Parameters.AddWithValue("@megn", uj.Megnevezes);
                uj.Id = (int)command.ExecuteScalar();
                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    if (command.Transaction != null)
                    {
                        command.Transaction.Rollback();
                    }

                }
                catch (Exception ex2)
                {
                    throw new ABKivetel("Vegzetes hiba az adatbazisban! Ertesitse a rendszergazdat!", ex2);
                }
                throw new ABKivetel("Sikertelen kolcsonzo felvitel!", ex);
            }
            finally
            {
                KapcsolatBontas();
            }
        }

        public static void KolcsonzoModositas(Kolcsonzo modosit)
        {
            Csatlakozas();
            try
            {
                command.Transaction = connection.BeginTransaction();
                TelepulesSzinkronizalasa(modosit.Cim);
                command.Parameters.Clear();
                command.CommandText = "UPDATE [Kolcsonzo] SET [Megnevezes] = @megn, [Kozterulet] = @kozt, [KozteruletJellege] = @koztj, [Hazszam] = @hsz, [IRSZ] = @irsz WHERE [Id] = @id";
                command.Parameters.AddWithValue("@megn", modosit.Megnevezes);
                command.Parameters.AddWithValue("@kozt", modosit.Cim.Kozterulet);
                command.Parameters.AddWithValue("@koztj", (int)modosit.Cim.KozteruletJellege);
                command.Parameters.AddWithValue("@hsz", modosit.Cim.Hazszam);
                command.Parameters.AddWithValue("@irsz", modosit.Cim.Irsz.ToString());
                command.ExecuteNonQuery();
                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    if (command.Transaction != null)
                    {
                        command.Transaction.Rollback();
                    }

                }
                catch (Exception ex2)
                {
                    throw new ABKivetel("Vegzetes hiba az adatbazisban! Ertesitse a rendszergazdat!", ex2);
                }
                throw new ABKivetel("Sikertelen kolcsonzo modositas!", ex);
            }
            finally
            {
                KapcsolatBontas();
            }
        }

        public static void KolcsonzoTorles(Kolcsonzo torol) { }

        public static void JarmuFelvitel(Kolcsonzo kolcsonzo, Jarmu uj) { }

        public static void JarmuModositas(Jarmu modosit) { }

        public static void JarmuTorles(Jarmu torol) { }

        public static List<Kolcsonzo> TeljesFelolvasas() { return null; }

    }
}