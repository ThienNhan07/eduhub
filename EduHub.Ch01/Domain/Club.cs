namespace EduHub.Ch01.Domain;

/// <summary>Câu lạc bộ. Danh sách thành viên chỉ được sửa qua AddMember / RemoveMember.</summary>
public class Club
{
    private readonly List<Member> _members = new();

    public int Id { get; private set; }
    public string Name { get; private set; }

    // Chỉ đọc: bên ngoài không thể Add/Remove trực tiếp
    public IReadOnlyList<Member> Members => _members;

    private Club(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Club Create(int id, string name)
    {
        if (id <= 0)
            throw new ArgumentException("Id phải lớn hơn 0.", nameof(id));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên CLB không được để trống.", nameof(name));

        return new Club(id, name.Trim());
    }

    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);
        if (!member.IsActive())
            throw new InvalidOperationException("Thành viên đã bị ngừng, không thể tham gia CLB.");
        if (_members.Any(m => m.Id == member.Id))
            throw new InvalidOperationException("Đã là thành viên của CLB.");

        _members.Add(member);
    }

    // US-02 (Sinh viên): rời CLB
    public void RemoveMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);
        var found = _members.FirstOrDefault(m => m.Id == member.Id)
            ?? throw new InvalidOperationException("Chưa là thành viên của CLB.");

        _members.Remove(found);
    }

    public bool HasMember(Member member) => _members.Any(m => m.Id == member.Id);
}
