//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CallerIdIntegration.EntityModel
{
    using System;
    
    public partial class uspSearchReservationTrips_Result
    {
        public int id { get; set; }
        public Nullable<int> DispatcherId { get; set; }
        public string PickUpAddress { get; set; }
        public string PickUpDate { get; set; }
        public string PickUpTime { get; set; }
        public string DropoffAddress { get; set; }
        public string car_no { get; set; }
        public Nullable<int> WebTripLogID { get; set; }
        public string AccountNo { get; set; }
        public string VoucherNo { get; set; }
        public string Price { get; set; }
        public string Details { get; set; }
        public string Telephone { get; set; }
        public Nullable<int> AmtOfPassengers { get; set; }
        public string DirectNotificationTime { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
