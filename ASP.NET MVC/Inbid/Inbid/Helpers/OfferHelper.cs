using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inbid.Models;

namespace Inbid.Helpers
{
    public class OfferHelper
    {
        public static List<OfferModel> OffersList(List<View_Offers> viewOffers, List<string> Onlines)
        {

            List<OfferModel> offersList = new List<OfferModel>();


            foreach (View_Offers o in viewOffers)
            {
                bool online = false;
                for (int i = 0; i < Onlines.Count; i++)
                {
                    if (o.UserName == Onlines[i])
                    {
                        offersList.Add(new OfferModel { OfferId = o.OfferId, Status = "online", UserName = o.UserName, StartPrice = o.StartPrice, CurrentPrice = o.CurrentPrice, Change = o.Change });
                        online = true;
                        break;
                    }
                }
                if (!online)
                {
                    offersList.Add(new OfferModel { OfferId = o.OfferId, Status = "offline", UserName = o.UserName, StartPrice = o.StartPrice, CurrentPrice = o.CurrentPrice, Change = o.Change });
                }

            }


            return offersList;
        }

        public static List<Status> OffersListJson(List<View_Offers> viewOffers, List<string> Onlines)
        {

            List<Status> statusList = new List<Status>();


            foreach (View_Offers o in viewOffers)
            {
                bool online = false;
                for (int i = 0; i < Onlines.Count; i++)
                {
                    if (o.UserName == Onlines[i])
                    {
                        statusList.Add(new Status(o.OfferId,  "online"));
                        online = true;
                        break;
                    }
                }
                if (!online)
                {
                    statusList.Add(new Status(o.OfferId,  "offline"));
                }

            }


            return statusList;
        }

    }
}