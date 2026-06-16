import { useState, useEffect, type ChangeEvent } from "react";
import { useNavigate } from "react-router";

const Register = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    fullName: "",
    homeCity: "",
    currentCity: "",
  });

  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (token) {
      navigate("/home");
    }
  }, [navigate]);

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;

    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleSubmit = () => {};
  return (
    <div className="min-h-screen bg-[#F8FAFC] flex flex-col justify-center items-center p-4 font-sans text-[#0F172A]">
      {/* Brand / Logo Area */}
      <div className="mb-8 text-center">
        <div className="w-24 h-12 bg-[#2563EB] rounded-xl mx-auto mb-4 flex items-center justify-center shadow-lg shadow-[#2563EB]/20">
          <span className="text-white text-xl font-bold tracking-tighter">
            Bridge
          </span>
        </div>
        <h2 className="text-3xl font-bold tracking-tight text-[#0F172A]">
          Make an account
        </h2>
        <p className="mt-2 text-[#64748B]">
          Join the community and discover the events you like.
        </p>
      </div>

      {/* Main Card Container */}
      <div className="w-full max-w-md bg-[#FFFFFF] rounded-2xl shadow-[0_8px_30px_rgb(0,0,0,0.04)] p-8 transition-all duration-300 hover:shadow-[0_8px_40px_rgb(0,0,0,0.08)]">
        <form onSubmit={handleSubmit} className="space-y-5">
          {/* Full Name Input */}
          <div className="space-y-1.5">
            <label
              htmlFor="fullName"
              className="block text-sm font-medium text-[#0F172A]"
            >
              Full Name
            </label>
            <input
              type="text"
              id="fullName"
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
              placeholder="Your full name"
              className="w-full px-4 py-3 bg-[#FFFFFF] border border-[#E2E8F0] rounded-xl text-[#0F172A] placeholder-[#64748B] focus:outline-none focus:ring-2 focus:ring-[#2563EB]/20 focus:border-[#2563EB] transition-all duration-200"
              required
            />
          </div>

          {/* Email Input */}
          <div className="space-y-1.5">
            <label
              htmlFor="email"
              className="block text-sm font-medium text-[#0F172A]"
            >
              Email Address
            </label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              placeholder="you@example.com"
              className="w-full px-4 py-3 bg-[#FFFFFF] border border-[#E2E8F0] rounded-xl text-[#0F172A] placeholder-[#64748B] focus:outline-none focus:ring-2 focus:ring-[#2563EB]/20 focus:border-[#2563EB] transition-all duration-200"
              required
            />
          </div>

          {/* Password Input */}
          <div className="space-y-1.5">
            <label
              htmlFor="password"
              className="block text-sm font-medium text-[#0F172A]"
            >
              Password
            </label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              placeholder="••••••••"
              className="w-full px-4 py-3 bg-[#FFFFFF] border border-[#E2E8F0] rounded-xl text-[#0F172A] placeholder-[#64748B] focus:outline-none focus:ring-2 focus:ring-[#2563EB]/20 focus:border-[#2563EB] transition-all duration-200"
              required
            />
          </div>

          {/* Submit Button */}
          <div className="pt-2">
            <button
              type="submit"
              className="w-full flex justify-center py-3.5 px-4 border border-transparent rounded-xl shadow-sm text-sm font-semibold text-white bg-[#2563EB] hover:bg-[#1d4ed8] focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-[#2563EB] transition-all duration-200 active:scale-[0.98]"
            >
              Sign Up
            </button>
          </div>
        </form>

        {/* Footer / Alt Actions */}
        <div className="mt-8 text-center text-sm text-[#64748B]">
          Already have an account?{" "}
          <a
            href="/login"
            className="font-semibold text-[#2563EB] hover:text-[#06B6D4] transition-colors duration-200"
          >
            Sign in
          </a>
        </div>
      </div>
    </div>
  );
};

export default Register;
