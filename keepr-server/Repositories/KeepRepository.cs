using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using keepr_server.Models;
using Dapper;

namespace keepr_server.Repositories
{
  public class KeepRepository
  {
    private readonly IDbConnection _db;
    public KeepRepository(IDbConnection db)
    {
      _db = db;
    }


    internal IEnumerable<Keep> GetAll()
    {
      string sql = @"
      SELECT 
      keep.*,
      prof.*
      FROM keeps keep
      JOIN profiles prof ON keep.creatorId = prof.id";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) =>
      {
        keep.Creator = profile;
        return keep;
      }, splitOn: "id");
    }

    internal Keep GetById(int id)
    {
      string sql = @" 
      SELECT 
      keep.*,
      prof.*
      FROM keeps keep
      JOIN profiles prof ON keep.creatorId = prof.id
      WHERE keep.id = @id";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) =>
      {
        keep.Creator = profile;
        return keep;
      }, new { id }, splitOn: "id").FirstOrDefault();
    }

    internal Keep Create(Keep newKeep)
    {
      string sql = @"
      INSERT INTO keeps 
      (name, description, img, views, shares, keeps, creatorId) 
      VALUES 
      (@Name, @Description, @Img, @Views, @Shares, @Keeps, @CreatorId);
      SELECT LAST_INSERT_ID();";
      int id = _db.ExecuteScalar<int>(sql, newKeep);
      newKeep.Id = id;
      return newKeep;
    }
    internal Keep Edit(Keep updated)
    {
      string sql = @"
        UPDATE keeps
        SET
         name = @Name,
         description = @Description,
         views = @Views,
         shares = @Shares,
         keeps = @Keeps
        WHERE id = @Id;";
      _db.Execute(sql, updated);
      return updated;
    }

    internal IEnumerable<Keep> GetKeepsByAccountId(string id)  {
      string sql =@"
      SELECT
      keep.*,
      profile.*
      FROM keeps keep
      JOIN profiles profile ON keep.creatorId = profile.id
      WHERE keep.creatorId = @id;";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) => {
        keep.Creator = profile;
        return keep;
      }, new { id }, splitOn: "id");
    }

        internal void Delete(int id)
    {
      string sql = "DELETE FROM keeps WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });
    }
    internal IEnumerable<KeepVaultViewModel> GetKeepsByVaultId(int id){
        string sql = @"
        SELECT
        k.*,
        vk.id AS VaultKeepId,
        profile.*
        FROM vaultkeeps vk
        JOIN keeps k ON k.id = vk.keepId
        JOIN profiles profile ON k.creatorId = profile.id
        WHERE vaultId = @id;";
        return _db.Query<KeepVaultViewModel, Profile, KeepVaultViewModel>(sql, (keep, profile) =>
      {
        keep.Creator = profile;
        return keep;
      }, new { id }, splitOn: "id");
    }
internal IEnumerable<KeepVaultViewModel> GetKeepsByProfileId(string id)
    {
      string sql = @"
      SELECT
      keep.*,
      profile.*
      FROM keeps keep
      JOIN profiles profile ON keep.creatorId = profile.id
      WHERE keep.creatorId = @id;";

      return _db.Query<KeepVaultViewModel, Profile, KeepVaultViewModel>(sql, (keep, profile) =>
      {
        keep.Creator = profile;
        return keep;
      }, new { id }, splitOn: "id");
    }
  }
}