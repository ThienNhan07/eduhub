namespace EduHub.Ch01.Domain;

/// <summary>Sự kiện của một CLB. ClubId là khoá ngoại trỏ về Club.</summary>
public class Event
{
    public int Id { get; private set; }
    public int ClubId { get; private set; }        // khoá ngoại
    public string Title { get; private set; }
    public int Capacity { get; private set; }
    public DateTime StartTime { get; private set; }
    public bool Cancelled { get; private set; }

    private Event(int id, int clubId, string title, int capacity, DateTime startTime)
    {
        Id = id;
        ClubId = clubId;
        Title = title;
        Capacity = capacity;
        StartTime = startTime;
    }

    public static Event Create(int id, int clubId, string title, int capacity, DateTime startTime)
    {
        if (id <= 0)
            throw new ArgumentException("Id phải lớn hơn 0.", nameof(id));
        if (clubId <= 0)
            throw new ArgumentException("Sự kiện phải thuộc về một CLB (ClubId > 0).", nameof(clubId));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề sự kiện không được để trống.", nameof(title));
        if (capacity <= 0)
            throw new ArgumentException("Capacity phải lớn hơn 0.", nameof(capacity));

        return new Event(id, clubId, title.Trim(), capacity, startTime);
    }

    // US-03 (Ban CLB): sự kiện đã bị hủy / đã diễn ra thì không nhận thêm thay đổi
    public void Cancel(DateTime now)
    {
        if (Cancelled)
            throw new InvalidOperationException("Sự kiện đã bị hủy trước đó.");
        if (StartTime <= now)
            throw new InvalidOperationException("Sự kiện đã diễn ra, không thể hủy.");

        Cancelled = true;
    }
}
