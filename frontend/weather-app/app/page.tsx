import Image from "next/image";
import Weather from "@/app/components/Weather";

export default function Home() {
  return (
    <div>
      <header>
        The Weather App
      </header>
      <div>
        <Weather />
      </div>
    </div>
  );
}
