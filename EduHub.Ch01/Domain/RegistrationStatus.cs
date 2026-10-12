namespace EduHub.Ch01.Domain;

/// <summary>Trạng thái phiếu đăng ký. Dùng enum thay cho chuỗi "Pending" rải khắp nơi.</summary>
public enum RegistrationStatus
{
    Pending,
    Confirmed,
    Cancelled
}
