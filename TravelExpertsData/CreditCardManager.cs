using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;

namespace TravelExpertsData
{
    

    public class CreditCardManager
    {
        // Get all credit cards by CustomerId
        public static List<CreditCardViewModel> GetCreditCardsForCustomer(TravelExpertsContext db, int customerId)
        {
            return db.CreditCards
                .Where(cc => cc.CustomerId == customerId)
                .Select(cc => new CreditCardViewModel
                {
                    CreditCardId = cc.CreditCardId,
                    CCName = cc.Ccname,
                    CCNumber = cc.Ccnumber,
                    CCExpiry = cc.Ccexpiry,
                    CustomerId = cc.CustomerId,
                    Balance = cc.Balance,
                    CreditCardImagePath = GetCardImagePath(cc.Ccname.ToLower())
                }).ToList();
        }

        // Get credit card by CreditCardId
        public static CreditCardViewModel GetCreditCardById(TravelExpertsContext db, int creditCardId)
        {
            var creditCard = db.CreditCards.FirstOrDefault(cc => cc.CreditCardId == creditCardId);
            if (creditCard == null)
            {
                return null;
            }

            return new CreditCardViewModel
            {
                CreditCardId = creditCard.CreditCardId,
                CCName = creditCard.Ccname,
                CCNumber = creditCard.Ccnumber,
                CCExpiry = creditCard.Ccexpiry,
                CustomerId = creditCard.CustomerId,
                Balance = creditCard.Balance,
                CreditCardImagePath = GetCardImagePath(creditCard.Ccname.ToLower())
            };
        }

        // Update credit card
        public static void UpdateCreditCard(TravelExpertsContext db, CreditCardViewModel creditCardViewModel)
        {
            var creditCard = db.CreditCards.FirstOrDefault(cc => cc.CreditCardId == creditCardViewModel.CreditCardId);
            if (creditCard != null)
            {
                creditCard.Ccname = creditCardViewModel.CCName;
                creditCard.Ccnumber = creditCardViewModel.CCNumber;
                creditCard.Ccexpiry = creditCardViewModel.CCExpiry;
                creditCard.CustomerId = creditCardViewModel.CustomerId;
                creditCard.Balance = creditCardViewModel.Balance;

                db.SaveChanges();
            }
        }

        public static string GetCardImagePath(string cardName)
        {
            return cardName switch
            {
                "visa" => "/images/visa.png",
                "mastercard" => "/images/mastercard.png",
                "amex" => "/images/amex.png",
                _ => "/images/default-card.png"
            };
        }
        public static bool DeductAmountAndUpdateBalance(TravelExpertsContext context, int creditCardId, decimal packageTotalPrice)
        {
            // Retrieve the credit card from the database
            var creditCard = context.CreditCards.SingleOrDefault(cc => cc.CreditCardId == creditCardId);

            if (creditCard == null)
            {
                throw new InvalidOperationException("Credit card not found.");
            }

            // Check if balance is sufficient
            if (creditCard.Balance >= packageTotalPrice)
            {
                // Deduct the package price from the balance
                creditCard.Balance -= packageTotalPrice;

                // Save changes to the database
                context.SaveChanges();

                return true; // Indicate that the deduction was successful
            }

            return false; // Insufficient funds
        }

    }

}
