using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Core
{
    public enum IssuerType
    {
        ForgotPassword,
        InviteBuyer,
        InviteSupplier,
        InviteNewRequest
    }

    public enum SortType
    {
        Ascending,
        Descending
    }

    public enum Role
    {
        administrator,
        buyers,
        suppliers
    }

    public enum BuyerPermission
    {
        run_report,
        view_all_supplier,
        create_form_template,
        create_new_template
    }
    
    public enum FormType
    {
        blank,
        template
    }
    public enum RequestStatus
    {
        pending = 1,
        inprogress = 2,
        completed = 3,        
        approved = 4,
        rejected = 5
    }
}
