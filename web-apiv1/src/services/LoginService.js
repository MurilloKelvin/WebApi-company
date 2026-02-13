import http from "@/http-common";

class LoginService {
  Login(username, password) {
    const query =
      "api-version=1.0" +
      `&username=${encodeURIComponent(username ?? "")}` +
      `&password=${encodeURIComponent(password ?? "")}`;

    return http
      .post(`/api/v1/auth?${query}`, null, {
        headers: { "Cache-Control": "no-cache" },
      })
      .then((response) => response.data);
  }
}

export default LoginService;
