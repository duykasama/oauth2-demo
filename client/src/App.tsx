import { useState } from 'react'
import './App.css'
import Home from './routes/home'
import Login from './routes/login'

function App() {
    const [isAuthenticated, setIsAuthenticated] = useState(false)

    return (
        <>
            {
                isAuthenticated ? <Home /> : <Login />
            }
        </>
    )
}

export default App
