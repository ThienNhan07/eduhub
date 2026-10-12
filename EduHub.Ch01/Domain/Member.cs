namespace EduHub.Ch01.Domain;

/// <summary>Thành viên (sinh viên) của hệ thống EduHub.</summary>
public class Member
{
    public int Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public bool Active { get; private set; }

    private Member(int id, string fullName, string email)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Active = true;
    }

    public static Member Create(int id, string fullName, string email)
    {
        if (id <= 0)
            throw new ArgumentException("Id phải lớn hơn 0.", nameof(id));
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Họ tên không được để trống.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email không hợp lệ.", nameof(email));

        return new Member(id, fullName.Trim(), email.Trim());
    }

    public bool IsActive() => Active;

    // US-05 (Admin): ngừng / kích hoạt lại thành viên, lịch sử đăng ký vẫn được giữ
    public void Deactivate()
    {
        if (!Active)
            throw new InvalidOperationException("Thành viên đã bị ngừng trước đó.");
        Active = false;
    }

    public void Reactivate()
    {
        if (Active)
            throw new InvalidOperationException("Thành viên đang hoạt động, không cần kích hoạt lại.");
        Active = true;
    }
}
