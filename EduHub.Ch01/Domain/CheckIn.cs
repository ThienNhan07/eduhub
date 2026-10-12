namespace EduHub.Ch01.Domain;

/// <summary>Bản ghi điểm danh. RegistrationId là khoá ngoại; chỉ phiếu Confirmed mới check-in được.</summary>
public class CheckIn
{
    public int Id { get; private set; }
    public int RegistrationId { get; private set; }    // khoá ngoại -> Registration
    public DateTime CheckedInAt { get; private set; }

    private CheckIn(int id, int registrationId, DateTime checkedInAt)
    {
        Id = id;
        RegistrationId = registrationId;
        CheckedInAt = checkedInAt;
    }

    public static CheckIn Create(int id, Registration registration, DateTime checkedInAt)
    {
        ArgumentNullException.ThrowIfNull(registration);
        if (id <= 0)
            throw new ArgumentException("Id phải lớn hơn 0.", nameof(id));
        if (registration.Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException(
                $"Chỉ phiếu Confirmed mới được check-in (hiện tại: {registration.Status}).");

        return new CheckIn(id, registration.Id, checkedInAt);
    }
}
