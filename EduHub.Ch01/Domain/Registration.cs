namespace EduHub.Ch01.Domain;

/// <summary>Phiếu đăng ký của một thành viên cho một sự kiện. EventId, MemberId là khoá ngoại.</summary>
public class Registration
{
    public int Id { get; private set; }
    public int EventId { get; private set; }       // khoá ngoại -> Event
    public int MemberId { get; private set; }      // khoá ngoại -> Member
    public RegistrationStatus Status { get; private set; }

    private Registration(int id, int eventId, int memberId)
    {
        Id = id;
        EventId = eventId;
        MemberId = memberId;
        Status = RegistrationStatus.Pending;
    }

    public static Registration Create(int id, int eventId, int memberId)
    {
        if (id <= 0)
            throw new ArgumentException("Id phải lớn hơn 0.", nameof(id));
        if (eventId <= 0)
            throw new ArgumentException("EventId không hợp lệ.", nameof(eventId));
        if (memberId <= 0)
            throw new ArgumentException("MemberId không hợp lệ.", nameof(memberId));

        return new Registration(id, eventId, memberId);
    }

    // US-03 (Ban CLB): xác nhận phiếu Pending
    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
            throw new InvalidOperationException($"Không thể xác nhận phiếu đang ở trạng thái {Status}.");

        Status = RegistrationStatus.Confirmed;
    }

    // US-01 (Sinh viên): hủy đăng ký
    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Phiếu đã bị hủy trước đó.");

        Status = RegistrationStatus.Cancelled;
    }
}
