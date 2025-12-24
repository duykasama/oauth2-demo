import { createFileRoute } from '@tanstack/react-router'
import HomePage from '../components/home-page'
import GoogleLoginPage from '../components/google-login-page'
import { useState } from 'react'

export const Route = createFileRoute('/' as any)({
    component: RouteComponent,
})

function RouteComponent() {
    const userInfo = JSON.parse(localStorage.getItem('userInfo') ?? '{}')
    const [isAuthenticated, _] = useState<boolean>(userInfo.name && userInfo.email && userInfo.picture)

    return (
        <>
            {
                isAuthenticated ? <HomePage /> : <GoogleLoginPage />
            }
        </>
    )
}
