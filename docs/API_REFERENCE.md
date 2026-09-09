# Leave Management API Reference

## Authentication
All endpoints require JWT Bearer token.

## Endpoints

### Employees
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/employees | List all employees |
| GET | /api/employees/{id} | Get employee details |
| POST | /api/employees | Create employee |
| PUT | /api/employees/{id} | Update employee |

### Leave Requests
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/leaves | List leave requests |
| POST | /api/leaves | Submit leave request |
| PUT | /api/leaves/{id}/approve | Approve leave |
| PUT | /api/leaves/{id}/reject | Reject leave |
| GET | /api/leaves/balance/{empId} | Check leave balance |

### Leave Types
| Type | Default Days | Carry Over |
|------|-------------|------------|
| Annual | 20 | Yes (max 5) |
| Sick | 10 | No |
| Personal | 5 | No |
| Maternity | 90 | N/A |

## Error Codes
| Code | Meaning |
|------|---------|
| 400 | Invalid request data |
| 401 | Authentication required |
| 403 | Insufficient permissions |
| 404 | Resource not found |
| 409 | Leave conflict (overlapping dates) |