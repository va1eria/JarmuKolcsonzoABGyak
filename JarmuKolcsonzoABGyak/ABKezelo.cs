using System;
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

        public static void KolcsonzoTorles(Kolcsonzo torol)
        {
            Csatlakozas();
            try
            {
                command.Transaction = connection.BeginTransaction();
                command.CommandText = "DELETE FROM [Auto] WHERE [Rendszam] IN (SELECT [Rendszam] FROM [Jarmu] WHERE [KolcsonzoId] = @id)";
                command.Parameters.AddWithValue("@id", torol.Id);
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM [Motor] WHERE [Rendszam] IN (SELECT [Rendszam] FROM [Jarmu] WHERE [KolcsonzoId] = @id)";
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM [Jarmu] WHERE [KolcsonzoId] = @id";
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM [Kolcsonzo] WHERE [Id] = @id";
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
                throw new ABKivetel("Sikertelen kolcsonzo torles!", ex);
            }
            finally
            {
                KapcsolatBontas();
            }
        }

        public static void JarmuFelvitel(Kolcsonzo kolcsonzo, Jarmu uj)
        {
            Csatlakozas();
            try
            {
                command.Transaction = connection.BeginTransaction();
                command.CommandText = "INSERT INTO [Jarmu] VALUES (@rend, @marka, @tipus, @fut, @kolcs, @kid)";
                command.Parameters.AddWithValue("@rend", uj.Rendszam);
                command.Parameters.AddWithValue("@marka", uj.Marka);
                command.Parameters.AddWithValue("@fut", uj.FutottKM);
                command.Parameters.AddWithValue("@kolcs", uj.Kolcsonozve);
                command.Parameters.AddWithValue("@kid", kolcsonzo.Id);
                command.ExecuteNonQuery();

                command.Parameters.Clear();
                command.Parameters.AddWithValue("@rend", uj.Rendszam);
                if (uj is Auto auto)
                {
                    command.CommandText = "INSERT INTO [Auto] VALUES (@rend, @atip, @szal)";
                    command.Parameters.AddWithValue("@atip", (int)auto.AutoTipusa);
                    command.Parameters.AddWithValue("@szal", auto.SzallithatoSzemSzam);
                }
                else if (uj is Motor motor)
                {
                    command.CommandText = "INSERT INTO [Motor] VALUES (@rend, @henger)";
                    command.Parameters.AddWithValue("@henger", motor.Hengerurtartalom);
                }
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
                throw new ABKivetel("Sikertelen jarmu felvitel!", ex);
            }
            finally
            {
                KapcsolatBontas();
            }
        }

        public static void JarmuModositas(Jarmu modosit)
        {
            Csatlakozas();
            try
            {
                command.CommandText = "UPDATE [Jarmu] SET [FutottKM] = @fut, [Kolcsonozve] = @kolcs WHERE [Rendszam] = @rend";
                command.Parameters.AddWithValue("@fut", modosit.FutottKM);
                command.Parameters.AddWithValue("@kolcs", modosit.Kolcsonozve);
                command.Parameters.AddWithValue("@rend", modosit.Rendszam);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen jarmu modositas!", ex);
            }
            finally
            {
                KapcsolatBontas();
            }
        }

        public static void JarmuTorles(Jarmu torol)
        {
            Csatlakozas();
            try
            {
                command.Transaction = connection.BeginTransaction();
                command.CommandText = $"DELETE FROM [{((torol is Auto) ? "Auto" : "Motor")}] WHERE [Rendszam] = @rend";
                command.Parameters.AddWithValue("@rend", torol.Rendszam);
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM [Jarmu] WHERE [Rendszam] = @rend";
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
                throw new ABKivetel("Sikertelen jarmu torles!", ex);
            }
            finally
            {
                KapcsolatBontas();
            }
        }

        public static List<Kolcsonzo> TeljesFelolvasas()
        {
            Csatlakozas();
            try
            {
                command.CommandText = "SELECT *, [Auto].[Rendszam] AS [Autorendszam], [Motor].[Rendszam] AS [Motorrendszam] FROM [Kolcsonzo] LEFT JOIN [Jarmu] ON [Kolcsonzo].[Id] = [Jarmu].[KolcsonzoId] LEFT JOIN [Telepules] ON [Kolcsonzo].[IRSZ] = [Telepules].[IRSZ] LEFT JOIN [Auto] ON [Jarmu].[Rendszam] = [Auto].[Rendszam] LEFT JOIN [Motor] ON [Jarmu].[Rendszam] = [Motor].[Rendszam]";
                List<Kolcsonzo> kolcsonzok = new List<Kolcsonzo>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (kolcsonzok.Count == 0 || kolcsonzok.Last().Id != (int)reader["Id"])
                        {
                            kolcsonzok.Add(new Kolcsonzo(
                                        (int)reader["Id"],
                                        reader["Megnevezes"].ToString(),
                                        new Cim(
                                            reader["Telepules"].ToString(),
                                            reader["Kozterulet"].ToString(),
                                            reader["Hazszam"].ToString(),
                                            short.Parse(reader["IRSZ"].ToString()),
                                            (KozteruletJelleg)(int)reader["KozteruletJellege"]
                                           )
                                ));
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("Autorendszam")))
                        {
                            kolcsonzok.Last().Jarmuvek.Add(new Auto(
                                    reader["Rendszam"].ToString(),
                                    reader["Marka"].ToString(),
                                    reader["Tipus"].ToString(),
                                    (int)reader["FutottKM"],
                                    bool.Parse(reader["Kolcsonozve"].ToString()),
                                    (AutoTipus)(int)reader["AutoTipusa"],
                                    (byte)reader["SzallithatoSzemSzam"]
                                ));
                        }
                        else if (!reader.IsDBNull(reader.GetOrdinal("Motorrendszam")))
                        {
                            kolcsonzok.Last().Jarmuvek.Add(new Motor(
                                    reader["Rendszam"].ToString(),
                                    reader["Marka"].ToString(),
                                    reader["Tipus"].ToString(),
                                    (int)reader["FutottKM"],
                                    bool.Parse(reader["Kolcsonozve"].ToString()),
                                    (double)reader["Hengerurtartalom"]
                                ));
                        }
                    }
                    reader.Close();
                }
                return kolcsonzok;
            }
            catch (Exception ex)
            {
                throw new ABKivetel("Sikertelen lekerdezes!", ex);
            }
            finally
            {
                KapcsolatBontas();
            }
        }

    }
}
