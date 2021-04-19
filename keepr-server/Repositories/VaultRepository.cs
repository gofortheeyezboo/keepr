using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using keepr_server.Models;
using Dapper;

namespace keepr_server.Repositories
{
  public class VaultRepository
  {
    private readonly IDbConnection _db;
    public VaultRepository(IDbConnection db)
    {
      _db = db;
    }


    internal IEnumerable<Vault> GetAll()
    {
      string sql = @"
      SELECT 
      vault.*,
      prof.*
      FROM vaults vault
      JOIN profiles prof ON vault.creatorId = prof.id";
      return _db.Query<Vault, Profile, Vault>(sql, (vault, profile) =>
      {
        vault.Creator = profile;
        return vault;
      }, splitOn: "id");
    }

    internal Vault GetById(int id)
    {
      string sql = @" 
      SELECT 
      vault.*,
      prof.*
      FROM vaults vault
      JOIN profiles prof ON vault.creatorId = prof.id
      WHERE vault.id = @id";
      return _db.Query<Vault, Profile, Vault>(sql, (vault, profile) =>
      {
        vault.Creator = profile;
        return vault;
      }, new { id }, splitOn: "id").FirstOrDefault();
    }

    internal Vault Create(Vault newVault)
    {
      string sql = @"
      INSERT INTO vaults 
      (name, description, isPrivate, creatorId) 
      VALUES 
      (@Name, @Description, @IsPrivate, @CreatorId);
      SELECT LAST_INSERT_ID();";
      int id = _db.ExecuteScalar<int>(sql, newVault);
      newVault.Id = id;
      return newVault;
    }
    internal Vault Edit(Vault updated)
    {
      string sql = @"
        UPDATE vaults
        SET
         name = @Name,
         description = @Description,
         isPrivate = @IsPrivate
        WHERE id = @Id;";
      _db.Execute(sql, updated);
      return updated;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM vaults WHERE id = @id LIMIT 1;";
      _db.Execute(sql, new { id });
    }
  }
}