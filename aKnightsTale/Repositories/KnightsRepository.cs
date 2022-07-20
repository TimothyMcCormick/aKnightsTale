using System.Collections.Generic;
using System.Data;
using System.Linq;
using aKnightsTale.Models;
using Dapper;


namespace aKnightsTale.Repositories
{
  public class KnightsRepository
  {
    private readonly IDbConnection _db;

    public KnightsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal List<Knight> Get()
    {
      string sql = @"
      SELECT
        k.*,
        acts.*
      FROM knights k
      JOIN accounts acts ON acts.id = k.creatorId
      ";
      return _db.Query<Knight, Profile, Knight>(sql, (knight, profile) =>
      {
        knight.Creator = profile;
        return knight;
      }).ToList();
    }

    internal Knight Get(int id)
    {
      string sql = @"
      SELECT
      knis.*,
      acts.*
      FROM knights knis
      JOIN accounts acts ON acts.id = knis.creatorId
      WHERE knis.id = @id
      ";
      return _db.Query<Knight, Profile, Knight>(sql, (knight, profile) =>
      {
        knight.Creator = profile;
        return knight;
      }, new { id }).FirstOrDefault();
    }

    internal Knight Create(Knight knightData)
    {
      string sql = @"
      INSERT INTO knights
      (name, creatorId)
      VALUES
      (@Name, @CreatorId);
      SELECT LAST_INSERT_ID();
      ";

      int id = _db.ExecuteScalar<int>(sql, knightData);
      knightData.Id = id;
      return knightData;
    }

    internal void Edit(Knight original)
    {
      string sql = @"
      UPDATE knights
      SET
        name = @Name
      WHERE id = @Id
      ";
      _db.Execute(sql, original);
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM knights WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}