using EduHub.Ch01.Domain;

// ============ EduHub - Console demo Ch01 ============
// 7 bước nghiệp vụ + bước 8: thử vi phạm luật nghiệp vụ.

var now = new DateTime(2026, 10, 12, 9, 0, 0);

Console.WriteLine("=== BƯỚC 1: Tạo CLB ===");
var club = Club.Create(1, "CLB Tin học");
Console.WriteLine($"Đã tạo: {club.Name} (Id = {club.Id})");

Console.WriteLine("\n=== BƯỚC 2: Tạo thành viên và tham gia CLB ===");
var members = new List<Member>
{
    Member.Create(1, "Nguyễn Văn An", "an@dthu.edu.vn"),
    Member.Create(2, "Trần Thị Bình", "binh@dthu.edu.vn"),
    Member.Create(3, "Lê Minh Châu", "chau@dthu.edu.vn"),
    Member.Create(4, "Phạm Quốc Dũng", "dung@dthu.edu.vn"),
};
foreach (var m in members)
{
    club.AddMember(m);
    Console.WriteLine($"{m.FullName} đã tham gia {club.Name}");
}
Console.WriteLine($"Tổng thành viên: {club.Members.Count}");

Console.WriteLine("\n=== BƯỚC 3: Ban CLB tạo sự kiện ===");
var ev = Event.Create(1, club.Id, "Workshop C# cơ bản", capacity: 3, startTime: now.AddDays(7));
Console.WriteLine($"Sự kiện: {ev.Title} | ClubId = {ev.ClubId} | Sức chứa = {ev.Capacity}");

Console.WriteLine("\n=== BƯỚC 4: Sinh viên đăng ký sự kiện ===");
var registrations = new List<Registration>();
int regId = 1;
foreach (var m in members.Take(3))
{
    var reg = Registration.Create(regId++, ev.Id, m.Id);
    registrations.Add(reg);
    Console.WriteLine($"Phiếu #{reg.Id} của {m.FullName}: {reg.Status}");
}

Console.WriteLine("\n=== BƯỚC 5: Ban CLB xác nhận phiếu ===");
registrations[0].Confirm();
registrations[1].Confirm();
foreach (var r in registrations)
    Console.WriteLine($"Phiếu #{r.Id}: {r.Status}");

Console.WriteLine("\n=== BƯỚC 6: Check-in tại sự kiện ===");
var checkIns = new List<CheckIn>();
var firstReg = registrations[0];
checkIns.Add(CheckIn.Create(1, firstReg, now.AddDays(7)));
Console.WriteLine($"Phiếu #{firstReg.Id} đã check-in lúc {checkIns[0].CheckedInAt:HH:mm dd/MM/yyyy}");

Console.WriteLine("\n=== BƯỚC 7: Báo cáo tham dự ===");
int confirmed = registrations.Count(r => r.Status == RegistrationStatus.Confirmed);
int attended = checkIns.Count;
double rate = confirmed == 0 ? 0 : attended * 100.0 / confirmed;
Console.WriteLine($"Sự kiện: {ev.Title}");
Console.WriteLine($"Đăng ký / sức chứa : {registrations.Count} / {ev.Capacity}");
Console.WriteLine($"Đã xác nhận        : {confirmed}");
Console.WriteLine($"Đã check-in        : {attended}");
Console.WriteLine($"Tỷ lệ tham dự      : {rate:0.#}%");
Console.WriteLine("Chi tiết từng người:");
foreach (var r in registrations)
{
    var member = members.First(m => m.Id == r.MemberId);
    bool done = checkIns.Any(c => c.RegistrationId == r.Id);
    Console.WriteLine($"  - {member.FullName,-18} [{r.Status,-9}] {(done ? "đã check-in" : "chưa check-in")}");
}

Console.WriteLine("\n=== BƯỚC 8: Thử vi phạm luật nghiệp vụ ===");
Try("Hủy phiếu rồi xác nhận lại", () =>
{
    registrations[1].Cancel();
    registrations[1].Confirm();
});
Try("Check-in bằng phiếu Pending", () => CheckIn.Create(2, registrations[2], now));
Try("Tham gia CLB lần 2", () => club.AddMember(members[0]));
Try("Tạo sự kiện capacity = 0", () => Event.Create(2, club.Id, "Sự kiện lỗi", 0, now.AddDays(1)));
Try("Tạo sự kiện tiêu đề trống", () => Event.Create(3, club.Id, "  ", 10, now.AddDays(1)));
Try("Kích hoạt lại thành viên đang hoạt động", () => members[0].Reactivate());

Console.WriteLine("\n=== Admin: ngừng rồi kích hoạt lại thành viên ===");
members[3].Deactivate();
Console.WriteLine($"{members[3].FullName}: IsActive() = {members[3].IsActive()}");
members[3].Reactivate();
Console.WriteLine($"{members[3].FullName}: IsActive() = {members[3].IsActive()}");

Console.WriteLine("\n=== Sinh viên rời CLB ===");
club.RemoveMember(members[2]);
Console.WriteLine($"Số thành viên còn lại: {club.Members.Count}");

static void Try(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($"[KHÔNG BỊ CHẶN] {title}");
    }
    catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
    {
        Console.WriteLine($"[Bị chặn] {title} -> {ex.Message}");
    }
}

