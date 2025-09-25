# Gmail Microservice

Dịch vụ microservice đơn giản để gửi email thông qua Gmail SMTP.

## Yêu cầu

- .NET SDK 8.0 hoặc cao hơn
- Tài khoản Gmail và [App Password](https://support.google.com/accounts/answer/185833?hl=en)

## Cấu hình

Cấu hình Gmail SMTP trong file `appsettings.json` hoặc `appsettings.Development.json`:

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-password",
    "SenderEmail": "your-email@gmail.com",
    "SenderName": "Your Name"
  }
}
```

**Lưu ý quan trọng:** 
- Không nên lưu trữ mật khẩu trong file cấu hình trực tiếp trong môi trường production
- Sử dụng user secrets, biến môi trường hoặc Azure Key Vault cho môi trường production
- Để sử dụng Gmail SMTP, bạn cần một "App Password" thay vì mật khẩu chính

## Chạy ứng dụng

```bash
cd GmailService
dotnet run
```

API sẽ chạy tại `http://localhost:5067` (HTTP) và `https://localhost:7095` (HTTPS)

## API Endpoints

### Gửi Email

```http
POST /email/send
```

**Request Body:**

```json
{
  "to": [
    "recipient@example.com"
  ],
  "cc": [
    "cc-recipient@example.com"
  ],
  "bcc": [],
  "subject": "Test Email from GmailService",
  "body": "<h1>Hello!</h1><p>This is a test email sent from the GmailService microservice.</p>",
  "isHtml": true,
  "attachments": []
}
```

**Attachments Format (nếu cần):**

```json
"attachments": [
  {
    "fileName": "document.pdf",
    "contentType": "application/pdf",
    "content": "base64-encoded-content"
  }
]
```

## Sử dụng trong project khác

1. Thêm dịch vụ này như một reference trong project chính
2. Hoặc gọi API endpoint từ bất kỳ ứng dụng nào có thể gọi HTTP requests