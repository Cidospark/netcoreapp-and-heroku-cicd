using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.Data
{
    public static class Seeder
    {
        public static async Task SeedIt(UMSDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // List of roles and claims
            var roles = new List<string>() { "Admin", "Member" };
            var allClaims = new List<string>() { "Can Create", "Can Delete", "Can Edit" };

            // loop through the list of roles, construct the role object and add it to the db
            if (!roleManager.Roles.Any())
            {
                try
                {
                    for (int i = 0; i < roles.Count; i++)
                    {
                        var role = new IdentityRole(roles[i]);
                        var roleResult = await roleManager.CreateAsync(role);
                        if (!roleResult.Succeeded)
                        {
                            HandleResultErrorMessag(roleResult, roles[i]);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }


            // if no claims values found in the database, then create some
            if (!context.ClaimsValues.Any())
            {
                try
                {
                    for (int i = 0; i < allClaims.Count; i++)
                    {
                        var singleClaim = new ClaimValue { ClaimsType = allClaims[i], ClaimsValue = allClaims[i] };
                        context.ClaimsValues.Add(singleClaim);
                        var roleResult = await context.SaveChangesAsync();
                        if (roleResult <= 0)
                        {
                            throw new Exception($"Failed to add claim value : {allClaims[i]} to the database");
                        }
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }


            // Get users from json file
            var usersData = await System.IO.File.ReadAllTextAsync("Data/seedData.json");
            var users = JsonConvert.DeserializeObject<List<AppUser>>(usersData);

            // add users to database
            if (!userManager.Users.Any())
            {
                int counter = 1;
                IdentityResult result = null;
                try
                {
                    foreach (var user in users)
                    {
                        if (counter < 2)
                        {
                            result = await userManager.CreateAsync(user, "My@dm1n");
                            counter += 1;
                            if (!result.Succeeded)
                            {
                                HandleResultErrorMessag(result, "admin");
                            }
                            await userManager.AddToRoleAsync(user, roles[0]);
                            foreach(var item in allClaims)
                            {
                                await userManager.AddClaimAsync(user, new Claim(item, "true"));
                            }
                        }
                        else
                        {
                            result = await userManager.CreateAsync(user, "P@$$w0rd");
                            if (!result.Succeeded)
                            {
                                HandleResultErrorMessag (result, "member");
                            }
                            await userManager.AddToRoleAsync(user, roles[1]);
                            await userManager.AddClaimAsync(user, new Claim(allClaims[2], "true"));
                        }
                    }
                }catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

        }

        // Helper method to handle error generated on creating users
        private static void HandleResultErrorMessag(IdentityResult result, string str)
        {
            var errMsg = "";
            foreach (var err in result.Errors)
            {
                errMsg += err.Description + ", ";
            }
            throw new Exception($"Failed to preseed {str}. {errMsg}");
        }
    }
}
