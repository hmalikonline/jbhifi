import Image from "next/image";
import Weather from "@/app/components/Weather";

export default function Home() {
  return (
    <div>      
      <header className="page-header">
        <div className="page-header-content-container" >
        The Weather App
        </div>
      </header>
      <div className="page-content-container">
        <Weather />
      </div>
    </div>
  );
}
