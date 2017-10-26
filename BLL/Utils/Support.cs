namespace BLL.Utils
{
    public static class Support
    {
        public enum PrescriptionsSortOrder
        {
            IdDesc, DrugName, DrugNameDesc, PrescriptionCreationDate, PrescriptionCreationDateDesc, PatientId, PatientIdDesc, PatientName, PatientNameDesc, PatientLastName, PatientLastNameDesc, PaymentMethod, PaymentMethodDesc
        }

        public enum PatientsSortOrder
        {
            PatientIdDesc, PatientName, PatientNameDesc, PatientLastName, PatientLastNameDesc, PatientBirthDate, PatientBirthDateDesc, PatientPhoneNumber, PatientPhoneNumberDesc
        }
    }
}
