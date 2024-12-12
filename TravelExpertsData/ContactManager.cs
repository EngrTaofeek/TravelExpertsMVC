using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;

namespace TravelExpertsData
{
    public class ContactManager
    {
        // Fetch all agencies with their agents
        public static List<ContactViewModel> GetAgenciesWithAgents(TravelExpertsContext db)
        {
            return db.Agencies
                .Include(a => a.Agents)
                .Select(a => new ContactViewModel
                {
                    AgencyId = a.AgencyId,
                    Address = a.AgncyAddress,
                    City = a.AgncyCity,
                    Province = a.AgncyProv,
                    PostalCode = a.AgncyPostal,
                    Phone = a.AgncyPhone,
                    Fax = a.AgncyFax,
                    Agents = a.Agents.Select(agent => new AgentViewModel
                    {
                        AgentId = agent.AgentId,
                        FirstName = agent.AgtFirstName,
                        LastName = agent.AgtLastName,
                        BusinessPhone = agent.AgtBusPhone,
                        Email = agent.AgtEmail,
                        Position = agent.AgtPosition
                    }).ToList()
                }).ToList();
        }
    }
}

